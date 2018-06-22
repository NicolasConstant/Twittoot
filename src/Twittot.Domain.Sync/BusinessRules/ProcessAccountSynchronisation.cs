﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twittoot.Mastodon.Std;
using Twittoot.Twitter.Setup;
using Twittoot.Twitter.Setup.Dtos;
using Twittot.Domain.Sync.Models;
using Twittot.Domain.Sync.Repositories;

namespace Twittot.Domain.Sync.BusinessRules
{
    public class ProcessAccountSynchronisation
    {
        private readonly SyncAccount _syncAccount;
        private readonly ITwitterSyncService _twitterService;
        private readonly IMastodonSyncService _mastodonService;
        private readonly ISyncAccountsRepository _syncAccountsRepository;

        public ProcessAccountSynchronisation(SyncAccount syncAccount, ITwitterSyncService twitterService, IMastodonSyncService mastodonService, ISyncAccountsRepository syncAccountsRepository)
        {
            this._syncAccount = syncAccount;
            _twitterService = twitterService;
            _mastodonService = mastodonService;
            _syncAccountsRepository = syncAccountsRepository;
        }

        public async Task Execute()
        {
            //Get tweets
            var lastTweets = GetTweetsUntilLastSync(_syncAccount.LastSyncTweetId).OrderBy(x => x.Id).ToList();

            //Sync
            if (lastTweets.Count == 0) return;
            foreach (var lastTweet in lastTweets)
            {
                var mediasIds = new int[0];
                var messageContent = lastTweet.MessageContent;

                if (lastTweet.MediaUrls != null)
                {
                    var uploadResults = (await _mastodonService.SubmitAttachementsAsync(_syncAccount.MastodonAccessToken, _syncAccount.MastodonInstance, lastTweet.MediaUrls)).ToArray();
                    mediasIds = uploadResults.Where(x => x.UploadSucceeded).Select(x => x.AttachementId).ToArray();

                    var failedUploadAttachementUrls = uploadResults.Where(x => !x.UploadSucceeded).Select(x => x.AttachementUrl);
                    foreach (var url in failedUploadAttachementUrls)
                        messageContent += $" {url}";
                }

                await _mastodonService.SubmitTootAsync(_syncAccount.MastodonAccessToken, _syncAccount.MastodonInstance, messageContent, mediasIds);
            }

            //Update profile
            _syncAccount.LastSyncTweetId = lastTweets.Select(x => x.Id).Max();
            _syncAccountsRepository.UpdateAccount(_syncAccount);
        }

        private IEnumerable<ExtractedTweet> GetTweetsUntilLastSync(long lastSyncTweetId)
        {
            var firstTweets = GetTweets(5).ToList();

            //First time synchronisation
            if (lastSyncTweetId == -1) return firstTweets.FindAll(x => IsNotAutoRetweet(x));

            //Retrieve all tweets until last sync
            var allTweets = new List<ExtractedTweet>();
            allTweets.AddRange(firstTweets);
            while (!allTweets.Any(x => x.Id <= lastSyncTweetId))
            {
                var nextTweets = GetTweets(50, allTweets.Select(x => x.Id).Min());
                allTweets.AddRange(nextTweets);
            }

            return allTweets.FindAll(x => x.Id > lastSyncTweetId && IsNotAutoRetweet(x));
        }

        private ExtractedTweet[] GetTweets(int nbTweets, long lastTweetId = -1)
        {
            return _twitterService.GetUserTweets(_syncAccount.TwitterName, nbTweets, false, lastTweetId);
        }

        private bool IsNotAutoRetweet(ExtractedTweet tweet)
        {
            return !tweet.MessageContent.Trim().StartsWith($"[RT {_syncAccount.TwitterName}]");
        }

        //private bool IsNotTweetResponse(ExtractedTweet tweet)
        //{
        //    return !tweet.MessageContent.Trim().StartsWith("@");
        //}
    }
}
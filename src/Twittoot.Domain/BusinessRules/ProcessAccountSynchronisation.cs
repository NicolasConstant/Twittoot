using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Twittoot.Domain.Models;
using Twittoot.Domain.Repositories;
using Twittoot.Mastodon;
using Twittoot.Twitter;
using Twittoot.Twitter.Dtos;

namespace Twittoot.Domain.BusinessRules
{
    public class ProcessAccountSynchronisation
    {
        private readonly SyncAccount _syncAccount;
        private readonly ITwitterService _twitterService;
        private readonly IMastodonService _mastodonService;
        private readonly ISyncAccountsRepository _syncAccountsRepository;

        public ProcessAccountSynchronisation(SyncAccount syncAccount, ITwitterService twitterService, IMastodonService mastodonService, ISyncAccountsRepository syncAccountsRepository)
        {
            this._syncAccount = syncAccount;
            _twitterService = twitterService;
            _mastodonService = mastodonService;
            _syncAccountsRepository = syncAccountsRepository;
        }

        public void Execute()
        {
            //Get tweets
            var lastTweets = GetTweetsUntilLastSync(_syncAccount.LastSyncTweetId).OrderBy(x => x.Id).ToList();

            //Sync
            if (lastTweets.Count == 0) return;
            foreach (var lastTweet in lastTweets)
            {
                var mediasIds = new int[0];
                if (lastTweet.MediaUrls != null)
                    mediasIds = _mastodonService.SubmitAttachements(_syncAccount.MastodonAccessToken, _syncAccount.MastodonInstance,
                        lastTweet.MediaUrls).ToArray();

                _mastodonService.SubmitToot(_syncAccount.MastodonAccessToken, _syncAccount.MastodonInstance,
                    lastTweet.MessageContent, mediasIds);
            }

            //Update profile
            _syncAccount.LastSyncTweetId = lastTweets.Select(x => x.Id).Max();
            _syncAccountsRepository.UpdateAccount(_syncAccount);
        }

        private IEnumerable<ExtractedTweet> GetTweetsUntilLastSync(long lastSyncTweetId)
        {
            var firstTweets = GetTweets(5).ToList();

            //First time synchronisation
            if (lastSyncTweetId == -1) return firstTweets.FindAll(IsNotAutoRetweet);

            //Retrieve all tweets until last sync
            var allTweets = new List<ExtractedTweet>();
            allTweets.AddRange(firstTweets);
            while (allTweets.All(x => x.Id != lastSyncTweetId))
            {
                var nextTweets = GetTweets(50, allTweets.Select(x => x.Id).Min());
                allTweets.AddRange(nextTweets);
            }

            return allTweets.FindAll(x => x.Id > lastSyncTweetId && IsNotAutoRetweet(x));
        }

        private ExtractedTweet[] GetTweets(int nbTweets, long lastTweetId = -1)
        {
            return _twitterService.GetUserTweets(_syncAccount.TwitterName, nbTweets, lastTweetId);
        }

        private bool IsNotAutoRetweet(ExtractedTweet tweet)
        {
            return !tweet.MessageContent.StartsWith($"[RT] @{_syncAccount.TwitterName}");
        }
    }
}
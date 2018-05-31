using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;
using Twittoot.Domain.Models;
using Twittoot.Domain.Repositories;
using Twittoot.Mastodon;
using Twittoot.Mastodon.Models;
using Twittoot.Twitter;

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
            var lastTweets = GetTweetsUntilLastSync(_syncAccount.LastSyncTweetId).Select(ExtractTweet).OrderBy(x => x.Id).ToList();

            //Sync
            if (lastTweets.Count == 0) return;
            foreach (var lastTweet in lastTweets)
                _mastodonService.SubmitToot(_syncAccount.MastodonAccessToken, _syncAccount.MastodonName, _syncAccount.MastodonInstance, lastTweet.MessageContent);

            //Update profile
            _syncAccount.LastSyncTweetId = lastTweets.Select(x => x.Id).Max();
            _syncAccountsRepository.UpdateAccount(_syncAccount);
        }

        private IEnumerable<ITweet> GetTweetsUntilLastSync(long lastSyncTweetId)
        {
            var firstTweets = GetTweets(5);

            //First time synchronisation
            if (lastSyncTweetId == -1) return firstTweets;

            //Retrieve all tweets until last sync
            var allTweets = new List<ITweet>();
            allTweets.AddRange(firstTweets);
            while (allTweets.All(x => x.Id != lastSyncTweetId))
            {
                var nextTweets = GetTweets(50, allTweets.Select(x => x.Id).Min());
                allTweets.AddRange(nextTweets);
            }

            return allTweets.FindAll(x => x.Id > lastSyncTweetId);
        }

        private ITweet[] GetTweets(int nbTweets, long lastTweetId = -1)
        {
            return _twitterService.GetUserTweets(_syncAccount.TwitterName, nbTweets, lastTweetId);
        }

        private ExtractedTeet ExtractTweet(ITweet tweet)
        {
            var tweetUrls = tweet.Media.Select(x => x.URL).Distinct();

            var message = tweet.FullText;
            foreach (var tweetUrl in tweetUrls)
                message = message.Replace(tweetUrl, string.Empty).Trim();

            return new ExtractedTeet
            {
                Id = tweet.Id,
                MessageContent = message,
                MediaUrls = tweet.Media.Select(x => x.MediaURLHttps).ToArray()
            };
        }
    }

    public class ExtractedTeet
    {
        public long Id { get; set; }
        public string MessageContent { get; set; }
        public string[] MediaUrls { get; set; }
    }
}
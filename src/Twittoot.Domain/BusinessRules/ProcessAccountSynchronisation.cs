using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;
using Twittoot.Domain.Models;
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

        public ProcessAccountSynchronisation(SyncAccount syncAccount, ITwitterService twitterService, IMastodonService mastodonService)
        {
            this._syncAccount = syncAccount;
            _twitterService = twitterService;
            _mastodonService = mastodonService;
        }

        public void Execute()
        {
            var getLastTweets = GetTweetsUntilLastSync(_syncAccount.LastSyncTweetId).OrderBy(x => x.Id).ToList();

            if (getLastTweets.Count == 0) return;

            foreach (var lastTweet in getLastTweets)
            {
                _mastodonService.SubmitToot(_syncAccount.MastodonAccessToken, _syncAccount.MastodonName, _syncAccount.MastodonInstance, lastTweet.FullText);
            }

            //TODO save last synchred tweet id
        }

        private IEnumerable<ITweet> GetTweetsUntilLastSync(long lastSyncTweetId)
        {
            var firstTweets = GetTweets();

            //First time synchronisation
            if (lastSyncTweetId == -1) return firstTweets;

            //Retrieve all tweets until last sync
            var allTweets = new List<ITweet>();
            allTweets.AddRange(firstTweets);
            while (allTweets.All(x => x.Id != lastSyncTweetId))
            {
                var nextTweets = GetTweets(allTweets.Select(x => x.Id).Min());
                allTweets.AddRange(nextTweets);
            }

            return allTweets.FindAll(x => x.Id > lastSyncTweetId);
        }

        private ITweet[] GetTweets(long lastTweetId = -1)
        {
            return _twitterService.GetUserTweets(_syncAccount.TwitterName, 50, lastTweetId);
        }
    }
}
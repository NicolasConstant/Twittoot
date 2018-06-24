using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;
using Twittoot.Twitter.Setup.Dtos;
using Twittoot.Twitter.Std.Repositories;

namespace Twittoot.Twitter.Setup
{
    public interface ITwitterSyncService
    {
        ExtractedTweet[] GetUserTweets(string twitterUserName, int nberTweets, bool returnReplies = true, long fromTweetId = -1);
    }

    public class TwitterSyncService : ITwitterSyncService
    {
        private readonly ITwitterUserSettingsRepository _twitterUserSettingsRepository;
        private readonly ITwitterDevSettingsRepository _twitterDevSettingsRepository;

        #region Ctor
        public TwitterSyncService(ITwitterUserSettingsRepository twitterUserSettingsRepository, ITwitterDevSettingsRepository twitterDevSettingsRepository)
        {
            _twitterUserSettingsRepository = twitterUserSettingsRepository;
            _twitterDevSettingsRepository = twitterDevSettingsRepository;
        }
        #endregion

        public ExtractedTweet[] GetUserTweets(string twitterUserName, int nberTweets, bool returnReplies = true, long fromTweetId = -1)
        {
            if(nberTweets > 200) 
                throw new ArgumentException("More than 200 Tweets retrieval isn't supported");

            var devSettings = _twitterDevSettingsRepository.GetTwitterDevApiSettings();
            var userSettings = _twitterUserSettingsRepository.GetTwitterUserApiSettings();
            
            Auth.SetUserCredentials(devSettings.ConsumerKey, devSettings.ConsumerSecret, userSettings.AccessToken, userSettings.AccessTokenSecret);
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            var user = User.GetUserFromScreenName(twitterUserName);

            var tweets = new List<ITweet>();
            if (fromTweetId == -1)
            {
                tweets.AddRange(Timeline.GetUserTimeline(user.Id, nberTweets));
            }
            else
            {
                var timelineRequestParameters = new UserTimelineParameters
                {
                    MaxId = fromTweetId - 1,
                    MaximumNumberOfTweetsToRetrieve = nberTweets
                };
                tweets.AddRange(Timeline.GetUserTimeline(user.Id, timelineRequestParameters));

            }

            return tweets.Where(x => returnReplies || string.IsNullOrWhiteSpace(x.InReplyToScreenName)).Select(ExtractTweet).ToArray();
        }

        private ExtractedTweet ExtractTweet(ITweet tweet)
        {
            var tweetUrls = tweet.Media.Select(x => x.URL).Distinct();

            var message = tweet.FullText;
            foreach (var tweetUrl in tweetUrls)
                message = message.Replace(tweetUrl, string.Empty).Trim();

            if (tweet.QuotedTweet != null) message = $"[Quote RT] {message}";
            if (tweet.IsRetweet)
            {
                if (tweet.RetweetedTweet != null) 
                    message = $"[RT {tweet.RetweetedTweet.CreatedBy.ScreenName}] {tweet.RetweetedTweet.FullText}";
                else
                    message = message.Replace("RT", "[RT]");
            }

            return new ExtractedTweet
            {
                Id = tweet.Id,
                MessageContent = message,
                MediaUrls = tweet.Media.Select(GetMediaUrl).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x).ToArray()
            };
        }

        private string GetMediaUrl(IMediaEntity media)
        {
            switch (media.MediaType)
            {
                case "photo": return media.MediaURLHttps;
                case "animated_gif": return media.VideoDetails.Variants[0].URL;
                case "video": return media.VideoDetails.Variants.OrderByDescending(x => x.Bitrate).First().URL;
                default: return null;
            }
        }
    }
}

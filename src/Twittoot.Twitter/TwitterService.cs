using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;
using Twittoot.Twitter.Dtos;
using Twittoot.Twitter.Repositories;
using Twittoot.Twitter.Settings;

namespace Twittoot.Twitter
{
    public interface ITwitterService
    {
        ExtractedTweet[] GetUserTweets(string twitterUserName, int nberTweets, long fromTweetId = -1);
        void EnsureTwitterIsReady();
    }

    public class TwitterService : ITwitterService
    {
        private readonly ITwitterSettingsRepository _twitterSettingsRepository;

        #region Ctor
        public TwitterService(ITwitterSettingsRepository twitterSettingsRepository)
        {
            _twitterSettingsRepository = twitterSettingsRepository;
        }
        #endregion

        public ExtractedTweet[] GetUserTweets(string twitterUserName, int nberTweets, long fromTweetId = -1)
        {
            if(nberTweets > 200) 
                throw new ArgumentException("More than 200 Tweets retrieval isn't supported");

            var devSettings = _twitterSettingsRepository.GetTwitterDevApiSettings();
            var userSettings = _twitterSettingsRepository.GetTwitterUserApiSettings();
            
            Auth.SetUserCredentials(devSettings.ConsumerKey, devSettings.ConsumerSecret, userSettings.AccessToken, userSettings.AccessTokenSecret);


            var user = User.GetUserFromScreenName(twitterUserName);

            if (fromTweetId == -1)
            {
                return Timeline.GetUserTimeline(user.Id, nberTweets).Select(ExtractTweet).ToArray();
            }
            else
            {
                var timelineRequestParameters = new UserTimelineParameters
                {
                    MaxId = fromTweetId - 1,
                    MaximumNumberOfTweetsToRetrieve = nberTweets
                };
                return Timeline.GetUserTimeline(user.Id, timelineRequestParameters).Select(ExtractTweet).ToArray();

            }
        }

        private ExtractedTweet ExtractTweet(ITweet tweet)
        {
            var tweetUrls = tweet.Media.Select(x => x.URL).Distinct();

            var message = tweet.FullText;
            foreach (var tweetUrl in tweetUrls)
                message = message.Replace(tweetUrl, string.Empty).Trim();

            if (tweet.QuotedTweet != null) message = $"[Quote RT] {message}";
            if (tweet.IsRetweet) message = message.Replace("RT", "[RT]");

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

        public void EnsureTwitterIsReady()
        {
            _twitterSettingsRepository.GetTwitterUserApiSettings();
            _twitterSettingsRepository.GetTwitterDevApiSettings();
        }
    }
}

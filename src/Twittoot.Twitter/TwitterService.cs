using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Twittoot.Twitter.Repositories;
using Twittoot.Twitter.Settings;

namespace Twittoot.Twitter
{
    public class TwitterService
    {
        private readonly ITwitterSettingsRepository _twitterSettingsRepository;

        #region Ctor
        public TwitterService(ITwitterSettingsRepository twitterSettingsRepository)
        {
            _twitterSettingsRepository = twitterSettingsRepository;
        }
        #endregion

        public ITweet[] GetUserTweets(string twitterUserName, int nberTweets, long fromTweetId = -1)
        {
            if(nberTweets > 200) 
                throw new ArgumentException("More than 200 Tweets retrieval isn't supported");

            var devSettings = _twitterSettingsRepository.GetTwitterDevApiSettings();
            var userSettings = _twitterSettingsRepository.GetTwitterUserApiSettings();
            
            Auth.SetUserCredentials(devSettings.ConsumerKey, devSettings.ConsumerSecret, userSettings.AccessToken, userSettings.AccessTokenSecret);


            var user = User.GetUserFromScreenName(twitterUserName);

            if (fromTweetId == -1)
            {
                return Timeline.GetUserTimeline(user.Id, nberTweets).ToArray();
            }
            else
            {
                var timelineRequestParameters = new UserTimelineParameters
                {
                    MaxId = fromTweetId - 1,
                    MaximumNumberOfTweetsToRetrieve = nberTweets
                };
                return Timeline.GetUserTimeline(user.Id, timelineRequestParameters).ToArray();

            }
        }
    }
}

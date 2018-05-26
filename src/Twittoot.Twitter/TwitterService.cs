using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Twittoot.Twitter.Repositories;
using Twittoot.Twitter.Settings;

namespace Twittoot.Twitter
{
    public class TwitterService
    {
        private readonly TwitterSettingsRepository _twitterSettingsRepository;

        #region Ctor
        public TwitterService(TwitterSettingsRepository twitterSettingsRepository)
        {
            _twitterSettingsRepository = twitterSettingsRepository;
        }
        #endregion

        public void GetUserTweets(string twitterUserName)
        {
            var devSettings = _twitterSettingsRepository.GetTwitterDevApiSettings();
            var userSettings = _twitterSettingsRepository.GetTwitterUserApiSettings();
            
            Auth.SetUserCredentials(devSettings.ConsumerKey, devSettings.ConsumerSecret, userSettings.AccessToken, userSettings.AccessTokenSecret);


            
        }
    }
}

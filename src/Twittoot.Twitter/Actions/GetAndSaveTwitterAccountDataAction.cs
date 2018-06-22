using System;
using Twittoot.Twitter.Setup.Settings;
using Twittoot.Twitter.Setup.Tools;
using Twittot.Twitter.Std.Repositories;

namespace Twittoot.Twitter.Setup.Actions
{
    public class GetAndSaveTwitterAccountDataAction
    {
        private readonly ITwitterUserSettingsRepository _twitterUserSettingsRepository;
        private readonly ITwitterDevSettingsRepository _twitterDevSettingsRepository;

        #region Ctor
        public GetAndSaveTwitterAccountDataAction(ITwitterUserSettingsRepository twitterUserSettingsRepository, ITwitterDevSettingsRepository twitterDevSettingsRepository)
        {
            _twitterUserSettingsRepository = twitterUserSettingsRepository;
            _twitterDevSettingsRepository = twitterDevSettingsRepository;
        }
        #endregion

        public void Execute()
        {
            var devSettings = _twitterDevSettingsRepository.GetTwitterDevApiSettings();
            if(devSettings == null) throw new Exception("Please set properly Twitter API keys.");

            var pinAuthenticator = new PinAuthenticator(devSettings);
            var creds = pinAuthenticator.GetTwitterCredentials();

            var userSettings = new TwitterUserApiSettings
            {
                AccessToken = creds.AccessToken,
                AccessTokenSecret = creds.AccessTokenSecret
            };

            _twitterUserSettingsRepository.SaveTwitterUserApiSettings(userSettings);
        }
    }
}
using System;
using System.Threading.Tasks;
using Twittoot.Twitter.Setup.Settings;
using Twittoot.Twitter.Setup.Tools;
using Twittoot.Twitter.Std.Repositories;

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

        public async Task ExecuteAsync()
        {
            var devSettings = await _twitterDevSettingsRepository.GetTwitterDevApiSettingsAsync();
            if(devSettings == null) throw new Exception("Please set properly Twitter API keys.");

            var pinAuthenticator = new PinAuthenticator(devSettings);
            var creds = pinAuthenticator.GetTwitterCredentials();

            var userSettings = new TwitterUserApiSettings
            {
                AccessToken = creds.AccessToken,
                AccessTokenSecret = creds.AccessTokenSecret
            };

            await _twitterUserSettingsRepository.SaveTwitterUserApiSettingsAsync(userSettings);
        }
    }
}
using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Twittoot.Twitter.Settings;
using Twittoot.Twitter.Tools;

namespace Twittoot.Twitter.Repositories
{
    public class TwitterSettingsRepository
    {
        private const string DevSettingsFileName = "Settings.Dev.json";
        private const string UserSettingsFileName = "Settings.User.json";
        private readonly TwitterDevApiSettings _defaultDevSettings = new TwitterDevApiSettings
        {
            ConsumerKey = "provide consumer key",
            ConsumerSecret = "provide consumer secret"
        };

        public TwitterDevApiSettings GetTwitterDevApiSettings()
        {
            var executingAsmDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var expectedDevSettingPath = Path.Combine(executingAsmDir, DevSettingsFileName);

            if (File.Exists(expectedDevSettingPath))
            {
                var fileContent = File.ReadAllText(expectedDevSettingPath);
                var devSettings = JsonConvert.DeserializeObject<TwitterDevApiSettings>(fileContent);
                if(devSettings.ConsumerKey == _defaultDevSettings.ConsumerKey && devSettings.ConsumerSecret == _defaultDevSettings.ConsumerSecret)
                    throw new Exception("Default Settings.Dev.json found, please provide correct info.");
                return devSettings;
            }
            else
            {
                var json = JsonConvert.SerializeObject(_defaultDevSettings);
                File.WriteAllText(expectedDevSettingPath, json);
                throw new Exception("Settings.Dev.json not found, please provide correct info.");
            }
        }

        public TwitterUserApiSettings GetTwitterUserApiSettings()
        {
            var executingAsmDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var expectedUserSettingPath = Path.Combine(executingAsmDir, UserSettingsFileName);

            if (File.Exists(expectedUserSettingPath))
            {
                var fileContent = File.ReadAllText(expectedUserSettingPath);
                var userSettings = JsonConvert.DeserializeObject<TwitterUserApiSettings>(fileContent);
                return userSettings;
            }
            else
            {
                var pinAuthenticator = new PinAuthenticator(GetTwitterDevApiSettings());
                var creds = pinAuthenticator.GetTwitterCredentials();

                var userSettings = new TwitterUserApiSettings
                {
                    AccessToken = creds.AccessToken,
                    AccessTokenSecret = creds.AccessTokenSecret
                };
                
                var json = JsonConvert.SerializeObject(userSettings);
                File.WriteAllText(expectedUserSettingPath, json);
                return userSettings;
            }
        }
    }
}
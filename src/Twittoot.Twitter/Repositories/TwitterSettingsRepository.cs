using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Twittoot.Twitter.Settings;

namespace Twittoot.Twitter.Repositories
{
    public class TwitterSettingsRepository
    {
        private const string DevSettingsFileName = "Settings.Dev.json";
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
            throw new NotImplementedException();
        }
    }
}
using System.IO;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public class TwitterUserSettingsFileRepository : ITwitterUserSettingsRepository
    {
        private const string UserSettingsFileName = "Settings.User.json";

        public TwitterUserApiSettings GetTwitterUserApiSettings()
        {
            var userSettingPath = TwittootLocation.GetUserFilePath(UserSettingsFileName);
            if (!File.Exists(userSettingPath)) return null;

            var fileContent = File.ReadAllText(userSettingPath);
            var userSettings = JsonConvert.DeserializeObject<TwitterUserApiSettings>(fileContent);
            return userSettings;
        }

        public void SaveTwitterUserApiSettings(TwitterUserApiSettings settings)
        {
            var userSettingPath = TwittootLocation.GetUserFilePath(UserSettingsFileName);
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(userSettingPath, json);
        }
    }
}
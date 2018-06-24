using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public class TwitterDevSettingsFileRepository : ITwitterDevSettingsRepository
    {
        private const string DevSettingsFileName = "Settings.Dev.json";
        
        public async Task<TwitterDevApiSettings> GetTwitterDevApiSettingsAsync()
        {
            var devSettingPath = TwittootLocation.GetUserFilePath(DevSettingsFileName);
            if (!File.Exists(devSettingPath)) return null;

            var fileContent = File.ReadAllText(devSettingPath);
            var devSettings = JsonConvert.DeserializeObject<TwitterDevApiSettings>(fileContent);
            return devSettings;
        }

        public async Task SaveTwitterDevApiSettingsAsync(TwitterDevApiSettings settings)
        {
            var devSettingPath = TwittootLocation.GetUserFilePath(DevSettingsFileName);
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(devSettingPath, json);
        }
    }
}
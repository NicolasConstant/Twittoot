using System.IO;
using Newtonsoft.Json;
using Twittoot.Twitter.Settings;

namespace Twittoot.Twitter.Tests
{
    public static class GetSettings
    {
        public static TwitterDevApiSettings GetDevSettings()
        {
            var pathToSecretFile = @"C:\Temp\Twitter.Dev.json";
            var json = File.ReadAllText(pathToSecretFile);
            return JsonConvert.DeserializeObject<TwitterDevApiSettings>(json);
        }

        public static TwitterUserApiSettings GetUserSettings()
        {
            var pathToSecretFile = @"C:\Temp\Twitter.User.json";
            var json = File.ReadAllText(pathToSecretFile);
            return JsonConvert.DeserializeObject<TwitterUserApiSettings>(json);
        }
    }
}
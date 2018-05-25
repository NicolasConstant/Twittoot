using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Twittoot.Twitter.Settings;
using Twittoot.Twitter.Tools;

namespace Twittoot.Twitter.Tests.Tools
{
    [TestClass]
    public class PinAuthenticatorTests
    {
        [TestMethod]
        public void GetCredentials()
        {
            var settings = GetDevSettings.GetSettings();
            var pinAuthenticator = new PinAuthenticator(settings);
            var creds = pinAuthenticator.GetTwitterCredentials();

        }
    }


    public static class GetDevSettings
    {
        public static TwitterDevApiSettings GetSettings()
        {
            var pathToSecretFile = @"C:\Temp\Twitter.json";
            var json = File.ReadAllText(pathToSecretFile);
            return JsonConvert.DeserializeObject<TwitterDevApiSettings>(json);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twittoot.Twitter.Tools;

namespace Twittoot.Twitter.Tests.Tools
{
    [TestClass]
    public class PinAuthenticatorTests
    {
        [TestMethod]
        public void GetCredentials()
        {
            var settings = GetSettings.GetDevSettings();
            var pinAuthenticator = new PinAuthenticator(settings);
            var creds = pinAuthenticator.GetTwitterCredentials();

        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twittoot.Twitter.Repositories;

namespace Twittoot.Twitter.Tests.Repositories
{
    [TestClass]
    public class TwitterSettingsRepositoryTests
    {
        [TestMethod]
        public void GetTwitterDevApiSettingsTest()
        {
            var repo = new TwitterSettingsRepository();
            var devApiSettings = repo.GetTwitterDevApiSettings();
        }

        [TestMethod]
        public void GetTwitterUserApiSettings()
        {
            var repo = new TwitterSettingsRepository();
            var userApiSettings = repo.GetTwitterUserApiSettings();
        }
    }
}
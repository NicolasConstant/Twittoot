using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Twittoot.Twitter.Repositories;

namespace Twittoot.Twitter.Tests
{
    [TestClass]
    public class TwitterServiceTests
    {
        [TestMethod]
        public void GetUserTweetsTest()
        {
            var twitterName = "globalstreetart";

            #region Mocks
            var repoMock = MockRepository.GenerateMock<ITwitterSettingsRepository>();
            repoMock.Expect(x => x.GetTwitterUserApiSettings()).Return(GetSettings.GetUserSettings());
            repoMock.Expect(x => x.GetTwitterDevApiSettings()).Return(GetSettings.GetDevSettings());
            #endregion

            var service = new TwitterService(repoMock);
            var completeBatch = service.GetUserTweets(twitterName, 50);

            var firstBatch = service.GetUserTweets(twitterName, 25);
            var secondBatch = service.GetUserTweets(twitterName, 25, firstBatch.Select(x => x.Id).Min());

            Assert.AreEqual(completeBatch[5].Url, firstBatch[5].Url);
            Assert.AreEqual(completeBatch[40].Url, secondBatch[15].Url);
        }
    }
}

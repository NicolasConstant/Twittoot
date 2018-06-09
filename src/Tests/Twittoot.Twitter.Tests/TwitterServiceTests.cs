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
            var secondBatch = service.GetUserTweets(twitterName, 25, false, firstBatch.Select(x => x.Id).Min());

            Assert.AreEqual(completeBatch[5].MessageContent, firstBatch[5].MessageContent);
            Assert.AreEqual(completeBatch[40].MessageContent, secondBatch[15].MessageContent);
        }
    }
}

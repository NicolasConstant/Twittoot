using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twittoot.Twitter.Oauth;

namespace Twittoot.Twitter.Tests.Oauth
{
    [TestClass]
    public class TwitterOauthTests
    {
        [TestMethod]
        public void ShowWindow()
        {
            var window = new TwitterOauth("http://google.com");
            window.ShowDialog();
        }
    }
}
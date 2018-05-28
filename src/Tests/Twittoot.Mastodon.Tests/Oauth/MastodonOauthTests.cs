using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twittoot.Mastodon.Oauth;

namespace Twittoot.Mastodon.Tests.Oauth
{
    [TestClass]
    public class MastodonOauthTests
    {
        [TestMethod]
        public void ShowWindow()
        {
            var url = $"https://mamot.fr/oauth/authorize?scope=read%20write%20follow&response_type=code&redirect_uri=urn:ietf:wg:oauth:2.0:oob&client_id=cc83c9ebbc2e817d6c71dc0c82f2c404e340ab7e96e62525526fe5d1c3e5fa9a";
            var window = new MastodonOauth(url);
            window.ShowDialog();
        }
    }
}
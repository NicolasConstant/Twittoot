using System;
using System.Diagnostics;
using Tweetinvi;
using Tweetinvi.Models;
using Twittoot.Twitter.Oauth;
using Twittoot.Twitter.Settings;

namespace Twittoot.Twitter.Tools
{
    public class PinAuthenticator
    {
        private readonly TwitterDevApiSettings _twitterDevApiSettings;

        #region Ctor
        public PinAuthenticator(TwitterDevApiSettings twitterDevApiSettings)
        {
            _twitterDevApiSettings = twitterDevApiSettings;
        }
        #endregion

        public ITwitterCredentials GetTwitterCredentials()
        {
            // Create a new set of credentials for the application.
            var appCredentials = new TwitterCredentials(_twitterDevApiSettings.ConsumerKey, _twitterDevApiSettings.ConsumerSecret);

            // Init the authentication process and store the related `AuthenticationContext`.
            var authenticationContext = AuthFlow.InitAuthentication(appCredentials);

            // Go to the URL so that Twitter authenticates the user and gives him a PIN code.
            var window = new TwitterOauth(authenticationContext.AuthorizationURL);
            window.ShowDialog();
            
            if(string.IsNullOrWhiteSpace(window.Code)) throw new Exception("User didn't succeed login");

            // With this pin code it is now possible to get the credentials back from Twitter
            var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(window.Code, authenticationContext);
            return userCredentials;

            // Use the user credentials in your application
            //Auth.SetCredentials(userCredentials);
        }
    }
}
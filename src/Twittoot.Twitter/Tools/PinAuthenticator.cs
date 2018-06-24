using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Twittoot.Twitter.Setup.Oauth;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Setup.Tools
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


            //SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var code = GetCode(authenticationContext.AuthorizationURL);



            //SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            //var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            //var code = "";
            //var t = Task.Factory.StartNew
            //(
            //    () =>
            //    {
            //        //Go to the URL so that Twitter authenticates the user and gives him a PIN code.
            //        var window = new TwitterOauth(authenticationContext.AuthorizationURL);
            //        window.ShowDialog();
            //        code = window.Code;
            //    },
            //    CancellationToken.None,
            //    TaskCreationOptions.None,
            //    scheduler
            //);

            //t.Wait();

            if (string.IsNullOrWhiteSpace(code)) throw new Exception("User didn't succeed login");

            // With this pin code it is now possible to get the credentials back from Twitter
            var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(code, authenticationContext);
            return userCredentials;


            // Use the user credentials in your application
            //Auth.SetCredentials(userCredentials);
        }

        private string GetCode(string url)
        {
            // Create a thread
            string code = null;
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                // Create and show the Window
                var tempWindow = new TwitterOauth(url);
                tempWindow.ShowDialog();
                code = tempWindow.Code;
                // Start the Dispatcher Processing
                System.Windows.Threading.Dispatcher.Run();
            }));
            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            // Start the thread
            newWindowThread.Start();

            while (code == null)
            {
                Thread.Sleep(100);
            }

            return code;
        }
    }
}
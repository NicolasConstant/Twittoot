using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twittoot.Twitter.Settings;

namespace Twittoot.Twitter
{
    public class TwitterService
    {
        private readonly TwitterDevApiSettings _twitterDevApiSettings;

        #region Ctor
        public TwitterService(TwitterDevApiSettings twitterDevApiSettings)
        {
            _twitterDevApiSettings = twitterDevApiSettings;
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twittoot.Mastodon
{
    public interface IMastodonService
    {
        void CreateAppInInstanceIfNotPresent(string mastodonInstance);
        string GetAccessToken(string mastodonName, string mastodonInstance);
    }

    public class MastodonService : IMastodonService
    {
        #region Ctor
        public MastodonService()
        {
            
        }
        #endregion

        public void CreateAppInInstanceIfNotPresent(string mastodonInstance)
        {
            throw new NotImplementedException();
        }

        public string GetAccessToken(string mastodonName, string mastodonInstance)
        {
            throw new NotImplementedException();
        }
    }
}

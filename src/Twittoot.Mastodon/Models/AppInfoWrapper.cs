using mastodon.Models;

namespace Twittoot.Mastodon.Models
{
    public class AppInfoWrapper : AppInfo
    {
        #region Ctor
        public AppInfoWrapper()
        {
            
        }
        
        public AppInfoWrapper(string instanceUrl, AppInfo appInfo)
        {
            id = appInfo.id;
            client_id = appInfo.client_id;
            client_secret = appInfo.client_secret;
            redirect_uri = appInfo.redirect_uri;
            InstanceUrl = instanceUrl;
        }
        #endregion

        public string InstanceUrl { get; set; }
    }
}
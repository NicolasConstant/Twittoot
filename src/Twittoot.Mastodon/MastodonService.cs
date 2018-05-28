using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mastodon;
using mastodon.Enums;
using Twittoot.Mastodon.Models;
using Twittoot.Mastodon.Oauth;
using Twittoot.Mastodon.Repositories;

namespace Twittoot.Mastodon
{
    public interface IMastodonService
    {
        AppInfoWrapper GetAppInfo(string mastodonInstance);
        string GetAccessToken(AppInfoWrapper appInfo, string mastodonName, string mastodonInstance);
    }

    public class MastodonService : IMastodonService
    {
        private readonly IInstancesRepository _instancesRepository;

        #region Ctor
        public MastodonService(IInstancesRepository instancesRepository)
        {
            _instancesRepository = instancesRepository;
        }
        #endregion

        public AppInfoWrapper GetAppInfo(string mastodonInstanceUrl)
        {
            var instances = _instancesRepository.GetAllInstances().ToList();
            if (instances.Any(x => x.InstanceUrl == mastodonInstanceUrl))
                return instances.Find(x => x.InstanceUrl == mastodonInstanceUrl);

            //Create new instance app
            var appHandler = new AppHandler(mastodonInstanceUrl);
            var scopes = AppScopeEnum.Read | AppScopeEnum.Write | AppScopeEnum.Follow;
            var appData = appHandler.CreateApp("Twittoot", "urn:ietf:wg:oauth:2.0:oob", scopes, "https://github.com/NicolasConstant/Twittoot");

            //Create new wrapper 
            var appDataWrapper = new AppInfoWrapper(mastodonInstanceUrl, appData);

            //Save it
            instances.Add(appDataWrapper);
            _instancesRepository.SaveInstances(instances.ToArray());

            //Returns 
            return appDataWrapper;
        }

        public string GetAccessToken(AppInfoWrapper appInfo, string mastodonName, string mastodonInstance)
        {
            var url = $"{mastodonInstance}/oauth/authorize?" + Uri.EscapeDataString($"scope=read write follow&response_type=code&redirect_uri=urn:ietf:wg:oauth:2.0:oob&client_id={appInfo.client_id}");
            var mastodonWindows = new MastodonOauth(url);
            mastodonWindows.Show();

            return mastodonWindows.Code;

        }
    }
}

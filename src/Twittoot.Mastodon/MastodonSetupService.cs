using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mastodon;
using mastodon.Enums;
using Twittoot.Mastodon.Setup.Oauth;
using Twittoot.Mastodon.Std.Models;
using Twittoot.Mastodon.Std.Repositories;

namespace Twittoot.Mastodon.Setup
{
    public interface IMastodonSetupService
    {
        Task<AppInfoWrapper> GetAppInfoAsync(string mastodonInstance);
        Task<string> GetAccessTokenAsync(AppInfoWrapper appInfo, string mastodonName, string mastodonInstance);
    }

    public class MastodonSetupService : IMastodonSetupService
    {
        private readonly IInstancesRepository _instancesRepository;
        //private readonly Dictionary<string, MastodonClient> _mastodonClientDict;

        #region Ctor
        public MastodonSetupService(IInstancesRepository instancesRepository)
        {
            _instancesRepository = instancesRepository;
            //_mastodonClientDict = new Dictionary<string, MastodonClient>();
        }
        #endregion

        public async Task<AppInfoWrapper> GetAppInfoAsync(string mastodonInstanceUrl)
        {
            var instances = _instancesRepository.GetAllInstances().ToList();
            if (instances.Any(x => x.InstanceUrl == mastodonInstanceUrl))
                return instances.Find(x => x.InstanceUrl == mastodonInstanceUrl);

            //Create new instance app
            var appHandler = new AppHandler(mastodonInstanceUrl);
            var scopes = AppScopeEnum.Read | AppScopeEnum.Write | AppScopeEnum.Follow;
            var appData = await appHandler.CreateAppAsync("Twittoot", scopes, "https://github.com/NicolasConstant/Twittoot");

            //Create new wrapper 
            var appDataWrapper = new AppInfoWrapper(mastodonInstanceUrl, appData);

            //Save it
            instances.Add(appDataWrapper);
            _instancesRepository.SaveInstances(instances.ToArray());

            //Returns 
            return appDataWrapper;
        }

        public async Task<string> GetAccessTokenAsync(AppInfoWrapper appInfo, string mastodonName, string mastodonInstance)
        {
            using (var authHandler = new AuthHandler(mastodonInstance))
            {
                //Get Oauth Code
                var oauthCodeUrl = authHandler.GetOauthCodeUrl(appInfo.client_id, AppScopeEnum.Read | AppScopeEnum.Write | AppScopeEnum.Follow);

                var code = "";
                var t = Task.Factory.StartNew
                (
                    () =>
                    {
                        var mastodonWindows = new MastodonOauth(oauthCodeUrl);
                        mastodonWindows.ShowDialog();
                        code = mastodonWindows.Code;
                    },
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
                
                t.Wait();

                //Get token
                var token = await authHandler.GetTokenInfoAsync(appInfo.client_id, appInfo.client_secret, code);
                return token.access_token;
            }
        }
    }
}

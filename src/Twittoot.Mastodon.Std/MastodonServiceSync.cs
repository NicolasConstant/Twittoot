using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using mastodon;
using mastodon.Enums;
using Twittoot.Mastodon.Std.Models;
using Twittoot.Mastodon.Std.Repositories;

namespace Twittoot.Mastodon.Std
{
    public interface IMastodonServiceSync
    {
        //Task<AppInfoWrapper> GetAppInfoAsync(string mastodonInstance);
        //Task<string> GetAccessTokenAsync(AppInfoWrapper appInfo, string mastodonName, string mastodonInstance);
        Task<IEnumerable<AttachementResult>> SubmitAttachementsAsync(string accessToken, string mastodonInstance, string[] attachementUrls);
        Task SubmitTootAsync(string accessToken, string mastodonInstance, string lastTweetFullText, int[] attachementsIds);
    }

    public class MastodonServiceSync : IMastodonServiceSync
    {
        private readonly IInstancesRepository _instancesRepository;
        private readonly Dictionary<string, MastodonClient> _mastodonClientDict;

        #region Ctor
        public MastodonServiceSync(IInstancesRepository instancesRepository)
        {
            _instancesRepository = instancesRepository;
            _mastodonClientDict = new Dictionary<string, MastodonClient>();
        }
        #endregion

        //public async Task<AppInfoWrapper> GetAppInfoAsync(string mastodonInstanceUrl)
        //{
        //    var instances = _instancesRepository.GetAllInstances().ToList();
        //    if (instances.Any(x => x.InstanceUrl == mastodonInstanceUrl))
        //        return instances.Find(x => x.InstanceUrl == mastodonInstanceUrl);

        //    //Create new instance app
        //    var appHandler = new AppHandler(mastodonInstanceUrl);
        //    var scopes = AppScopeEnum.Read | AppScopeEnum.Write | AppScopeEnum.Follow;
        //    var appData = await appHandler.CreateAppAsync("Twittoot", scopes, "https://github.com/NicolasConstant/Twittoot");

        //    //Create new wrapper 
        //    var appDataWrapper = new AppInfoWrapper(mastodonInstanceUrl, appData);

        //    //Save it
        //    instances.Add(appDataWrapper);
        //    _instancesRepository.SaveInstances(instances.ToArray());

        //    //Returns 
        //    return appDataWrapper;
        //}

        //public async Task<string> GetAccessTokenAsync(AppInfoWrapper appInfo, string mastodonName, string mastodonInstance)
        //{
        //    using (var authHandler = new AuthHandler(mastodonInstance))
        //    {
        //        //Get Oauth Code
        //        var oauthCodeUrl = authHandler.GetOauthCodeUrl(appInfo.client_id, AppScopeEnum.Read | AppScopeEnum.Write | AppScopeEnum.Follow);

        //        var code = "";
        //        var t = Task.Factory.StartNew
        //        (
        //            () =>
        //            {
        //                var mastodonWindows = new MastodonOauth(oauthCodeUrl);
        //                mastodonWindows.ShowDialog();
        //                code = mastodonWindows.Code;
        //            },
        //            CancellationToken.None,
        //            TaskCreationOptions.None,
        //            TaskScheduler.FromCurrentSynchronizationContext()
        //        );
                
        //        t.Wait();

        //        //Get token
        //        var token = await authHandler.GetTokenInfoAsync(appInfo.client_id, appInfo.client_secret, code);
        //        return token.access_token;
        //    }
        //}

        //public class OauthReturn
        //{
        //    public string access_token { get; set; }
        //    public string token_type { get; set; }
        //    public string scope { get; set; }
        //    public int created_at { get; set; }
        //}

        public async Task SubmitTootAsync(string accessToken, string mastodonInstance, string lastTweetFullText, int[] attachementsIds)
        {
            var client = GetClient(mastodonInstance);
            await client.PostNewStatusAsync(accessToken, lastTweetFullText, StatusVisibilityEnum.Public, -1, attachementsIds);
        }

        public async Task<IEnumerable<AttachementResult>> SubmitAttachementsAsync(string accessToken, string mastodonInstance, string[] attachementUrls)
        {
            var client = GetClient(mastodonInstance);

            var results = new List<AttachementResult>();
            foreach (var attachementUrl in attachementUrls)
            {
                int? id = null;
                var uploadSucceed = true;
                try
                {
                    byte[] imageBytes;
                    using (var webClient = new WebClient())
                    {
                        imageBytes = webClient.DownloadData(attachementUrl);
                    }

                    var uri = new Uri(attachementUrl);
                    var filename = Path.GetFileName(uri.AbsolutePath);

                    id = (await client.UploadingMediaAttachmentAsync(accessToken, filename, imageBytes, filename)).id; //TODO reintegrate failed URL to toot message
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    uploadSucceed = false;
                }

                var result = new AttachementResult()
                {
                    AttachementId = id ?? -1,
                    UploadSucceeded = uploadSucceed,
                    AttachementUrl = attachementUrl
                };
                results.Add(result);
            }

            return results;
        }

        private MastodonClient GetClient(string mastodonInstanceUrl)
        {
            if (!_mastodonClientDict.ContainsKey(mastodonInstanceUrl))
            {
                var newClient = new MastodonClient(mastodonInstanceUrl);
                _mastodonClientDict.Add(mastodonInstanceUrl, newClient);
            }

            return _mastodonClientDict[mastodonInstanceUrl];
        }
    }
}

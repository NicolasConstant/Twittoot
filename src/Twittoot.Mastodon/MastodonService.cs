﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using mastodon;
using mastodon.Enums;
using Newtonsoft.Json;
using Twittoot.Mastodon.Models;
using Twittoot.Mastodon.Oauth;
using Twittoot.Mastodon.Repositories;

namespace Twittoot.Mastodon
{
    public interface IMastodonService
    {
        AppInfoWrapper GetAppInfo(string mastodonInstance);
        string GetAccessToken(AppInfoWrapper appInfo, string mastodonName, string mastodonInstance);
        void SubmitToot(string accessToken, string mastodonName, string mastodonInstance, string lastTweetFullText);
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
            //Get Oauth Code
            var oauthCodeUrl = $"{mastodonInstance}/oauth/authorize?scope=read%20write%20follow&response_type=code&redirect_uri=urn:ietf:wg:oauth:2.0:oob&client_id={appInfo.client_id}";
            var mastodonWindows = new MastodonOauth(oauthCodeUrl);
            mastodonWindows.ShowDialog();

            //Get AccessToken
            var accessTokenUrl = $"{mastodonInstance}/oauth/token?client_id={appInfo.client_id}&client_secret={appInfo.client_secret}&grant_type=authorization_code&code={mastodonWindows.Code}&redirect_uri=urn:ietf:wg:oauth:2.0:oob";

            var client = new HttpClient();
            var response = client.PostAsync(accessTokenUrl, null).Result;
            var oauthReturnJson = response.Content.ReadAsStringAsync().Result;
            var oauthReturn = JsonConvert.DeserializeObject<OauthReturn>(oauthReturnJson);

            return oauthReturn.access_token;
        }

        public class OauthReturn
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string scope { get; set; }
            public int created_at { get; set; }
        }
        
        public void SubmitToot(string accessToken, string mastodonName, string mastodonInstance, string lastTweetFullText)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twittoot.Domain.Models;
using Twittoot.Domain.Repositories;
using Twittoot.Mastodon;
using Twittoot.Twitter;

namespace Twittoot.Domain
{
    public interface ITwittootFacade
    {
        void RegisterNewAccount(string twitterName, string mastodonName, string mastodonInstance);
        SyncAccount[] GetAllAccounts();
        void DeleteAccount(Guid accountId);
        void Run();
    }

    public class TwittootFacade : ITwittootFacade
    {
        private readonly ITwitterService _twitterService;
        private readonly IMastodonService _mastodonService;
        private readonly ISyncAccountsRepository _syncAccountsRepository;

        #region Ctor
        public TwittootFacade(ITwitterService twitterService, IMastodonService mastodonService, ISyncAccountsRepository syncAccountsRepository)
        {
            _twitterService = twitterService;
            _mastodonService = mastodonService;
            _syncAccountsRepository = syncAccountsRepository;
        }
        #endregion

        public void RegisterNewAccount(string twitterName, string mastodonName, string mastodonInstance)
        {
            //Ensure Twitter client is properly set
            _twitterService.EnsureTwitterIsReady();

            //Create mastodon profile
            var appInfo = _mastodonService.GetAppInfo(mastodonInstance);
            var userToken = _mastodonService.GetAccessToken(appInfo, mastodonName, mastodonInstance);

            var newSyncProfile = new SyncAccount
            {
                Id = Guid.NewGuid(),
                TwitterName = twitterName,
                MastodonName = mastodonName,
                MastodonInstance = mastodonInstance,
                MastodonToken = userToken,
                LastSyncTweetId = -1
            };

            //Save new profil
            var allAccounts = _syncAccountsRepository.GetAllAccounts().ToList();
            allAccounts.Add(newSyncProfile);
            _syncAccountsRepository.SaveAccounts(allAccounts.ToArray());
        }

        public SyncAccount[] GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
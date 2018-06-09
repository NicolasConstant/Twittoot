using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twittoot.Domain.BusinessRules;
using Twittoot.Domain.Factories;
using Twittoot.Domain.Models;
using Twittoot.Domain.Repositories;
using Twittoot.Mastodon;
using Twittoot.Twitter;

namespace Twittoot.Domain
{
    public interface ITwittootFacade
    {
        Task RegisterNewAccountAsync(string twitterName, string mastodonName, string mastodonInstance);
        SyncAccount[] GetAllAccounts();
        void DeleteAccount(Guid accountId);
        Task RunAsync();
    }

    public class TwittootFacade : ITwittootFacade
    {
        private readonly ITwitterService _twitterService;
        private readonly IMastodonService _mastodonService;
        private readonly ISyncAccountsRepository _syncAccountsRepository;
        private readonly ProcessAccountSyncFactory _processAccountSyncFactory;

        #region Ctor
        public TwittootFacade(ITwitterService twitterService, IMastodonService mastodonService, ISyncAccountsRepository syncAccountsRepository, ProcessAccountSyncFactory processAccountSyncFactory)
        {
            _twitterService = twitterService;
            _mastodonService = mastodonService;
            _syncAccountsRepository = syncAccountsRepository;
            _processAccountSyncFactory = processAccountSyncFactory;
        }
        #endregion

        public async Task RegisterNewAccountAsync(string twitterName, string mastodonName, string mastodonInstance)
        {
            //Ensure Twitter client is properly set
            _twitterService.EnsureTwitterIsReady();

            //Create mastodon profile
            var appInfo = await _mastodonService.GetAppInfoAsync(mastodonInstance);
            var userToken = await _mastodonService.GetAccessTokenAsync(appInfo, mastodonName, mastodonInstance);

            var newSyncProfile = new SyncAccount
            {
                Id = Guid.NewGuid(),
                TwitterName = twitterName,
                MastodonName = mastodonName,
                MastodonInstance = mastodonInstance,
                MastodonAccessToken = userToken,
                LastSyncTweetId = -1
            };

            //Save new profil
            var allAccounts = _syncAccountsRepository.GetAllAccounts().ToList();
            allAccounts.Add(newSyncProfile);
            _syncAccountsRepository.SaveAccounts(allAccounts.ToArray());
        }

        public SyncAccount[] GetAllAccounts()
        {
            return _syncAccountsRepository.GetAllAccounts();
        }

        public void DeleteAccount(Guid accountId)
        {
            var allAccounts = _syncAccountsRepository.GetAllAccounts().Where(x => x.Id != accountId).Select(x => x).ToList();
            _syncAccountsRepository.SaveAccounts(allAccounts.ToArray());
        }

        public async Task RunAsync()
        {
            var accounts = _syncAccountsRepository.GetAllAccounts();
            
            foreach (var syncAccount in accounts)
            {
                var action = _processAccountSyncFactory.GetAccountSync(syncAccount);
                await action.Execute();
            }
        }
    }
}
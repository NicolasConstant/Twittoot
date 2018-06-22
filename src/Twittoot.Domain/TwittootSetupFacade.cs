using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twittoot.Domain.Sync.Models;
using Twittoot.Domain.Sync.Repositories;
using Twittoot.Mastodon.Setup;
using Twittoot.Twitter;
using Twittoot.Twitter.Setup;

namespace Twittoot.Domain
{
    public interface ITwittootSetupFacade
    {
        Task RegisterNewAccountAsync();
        SyncAccount[] GetAllAccounts();
        void DeleteAccount(Guid accountId);
    }

    public class TwittootSetupFacade : ITwittootSetupFacade
    {
        private readonly ITwitterSetupService _twitterSetupService;
        private readonly IMastodonSetupService _mastodonService;
        private readonly ISyncAccountsRepository _syncAccountsRepository;

        #region Ctor
        public TwittootSetupFacade(ITwitterSetupService twitterSetupService, IMastodonSetupService mastodonService, ISyncAccountsRepository syncAccountsRepository)
        {
            _twitterSetupService = twitterSetupService;
            _mastodonService = mastodonService;
            _syncAccountsRepository = syncAccountsRepository;
        }
        #endregion

        public async Task RegisterNewAccountAsync()
        {
            //Ensure Twitter client is properly set
            var isTwitterSet = _twitterSetupService.IsTwitterSet();
            if(!isTwitterSet) _twitterSetupService.InitAndSaveTwitterAccount();

            //Create mastodon profile
            Console.WriteLine();
            Console.WriteLine("Provide Twitter Name");
            var twitterName = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Provide Mastodon Name");
            var mastodonName = Console.ReadLine();


            Console.WriteLine();
            Console.WriteLine("Provide Mastodon Instance");
            var mastodonInstance = Console.ReadLine();
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
    }
}
using Twittoot.Domain.Sync.BusinessRules;
using Twittoot.Domain.Sync.Models;
using Twittoot.Domain.Sync.Repositories;
using Twittoot.Mastodon.Std;
using Twittoot.Twitter.Setup;

namespace Twittoot.Domain.Sync.Factories
{
    public class ProcessAccountSyncFactory
    {
        private readonly ITwitterSyncService _twitterService;
        private readonly IMastodonSyncService _mastodonService;
        private readonly ISyncAccountsRepository _syncAccountsRepository;

        #region Ctor
        public ProcessAccountSyncFactory(ITwitterSyncService twitterService, IMastodonSyncService mastodonService, ISyncAccountsRepository syncAccountsRepository)
        {
            _twitterService = twitterService;
            _mastodonService = mastodonService;
            _syncAccountsRepository = syncAccountsRepository;
        }
        #endregion

        public ProcessAccountSynchronisation GetAccountSync(SyncAccount account)
        {
            return new ProcessAccountSynchronisation(account, _twitterService, _mastodonService, _syncAccountsRepository);
        }
    }
}
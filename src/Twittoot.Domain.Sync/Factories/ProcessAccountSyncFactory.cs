using Twittoot.Mastodon.Std;
using Twittoot.Twitter.Setup;
using Twittot.Domain.Sync.BusinessRules;
using Twittot.Domain.Sync.Models;
using Twittot.Domain.Sync.Repositories;

namespace Twittot.Domain.Sync.Factories
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
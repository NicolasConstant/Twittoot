using System.Threading.Tasks;
using Twittoot.Mastodon.Std;
using Twittoot.Twitter.Setup;
using Twittot.Domain.Sync.Factories;
using Twittot.Domain.Sync.Repositories;

namespace Twittot.Domain.Sync
{
    public interface ITwittootSyncFacade
    {
        Task RunAsync();
    }

    public class TwittootSyncFacade : ITwittootSyncFacade
    {
        private readonly ITwitterSyncService _twitterService;
        private readonly IMastodonSyncService _mastodonService;
        private readonly ISyncAccountsRepository _syncAccountsRepository;
        private readonly ProcessAccountSyncFactory _processAccountSyncFactory;

        #region Ctor
        public TwittootSyncFacade(ITwitterSyncService twitterService, IMastodonSyncService mastodonService, ISyncAccountsRepository syncAccountsRepository, ProcessAccountSyncFactory processAccountSyncFactory)
        {
            _twitterService = twitterService;
            _mastodonService = mastodonService;
            _syncAccountsRepository = syncAccountsRepository;
            _processAccountSyncFactory = processAccountSyncFactory;
        }
        #endregion

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
using Twittoot.Domain.Sync.Models;

namespace Twittoot.Domain.Sync.Repositories
{
    public interface ISyncAccountsRepository
    {
        SyncAccount[] GetAllAccounts();
        void UpdateAccount(SyncAccount account);
        void SaveAccounts(SyncAccount[] accounts);
    }
}
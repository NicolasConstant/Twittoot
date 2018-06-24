using System.Threading.Tasks;
using Twittoot.Domain.Sync.Models;

namespace Twittoot.Domain.Sync.Repositories
{
    public interface ISyncAccountsRepository
    {
        Task<SyncAccount[]> GetAllAccounts();
        Task UpdateAccount(SyncAccount account);
        Task SaveAccounts(SyncAccount[] accounts);
    }
}
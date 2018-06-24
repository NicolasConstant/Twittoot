using System.Threading.Tasks;
using Twittoot.Domain.Sync.Models;

namespace Twittoot.Domain.Sync.Repositories
{
    public interface ISyncAccountsRepository
    {
        Task<SyncAccount[]> GetAllAccountsAsync();
        Task UpdateAccountAsync(SyncAccount account);
        Task SaveAccountsAsync(SyncAccount[] accounts);
    }
}
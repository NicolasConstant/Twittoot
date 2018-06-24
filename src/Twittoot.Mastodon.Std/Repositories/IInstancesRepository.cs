using System.Threading.Tasks;
using Twittoot.Mastodon.Std.Models;

namespace Twittoot.Mastodon.Std.Repositories
{
    public interface IInstancesRepository
    {
        Task<AppInfoWrapper[]> GetAllInstancesAsync();
        Task SaveInstancesAsync(AppInfoWrapper[] instances);
    }
}
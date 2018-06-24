using System.Threading.Tasks;
using Twittoot.Mastodon.Std.Models;

namespace Twittoot.Mastodon.Std.Repositories
{
    public class InstancesAzureTableRepository : IInstancesRepository
    {
        public async Task<AppInfoWrapper[]> GetAllInstancesAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveInstancesAsync(AppInfoWrapper[] instances)
        {
            throw new System.NotImplementedException();
        }
    }
}
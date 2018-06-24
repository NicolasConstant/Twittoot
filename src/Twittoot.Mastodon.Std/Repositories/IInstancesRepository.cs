using Twittoot.Mastodon.Std.Models;

namespace Twittoot.Mastodon.Std.Repositories
{
    public interface IInstancesRepository
    {
        AppInfoWrapper[] GetAllInstances();
        void SaveInstances(AppInfoWrapper[] instances);
    }
}
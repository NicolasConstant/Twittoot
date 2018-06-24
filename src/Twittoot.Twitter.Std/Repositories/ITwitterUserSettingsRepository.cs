using System.Threading.Tasks;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public interface ITwitterUserSettingsRepository
    {
        Task<TwitterUserApiSettings> GetTwitterUserApiSettingsAsync();
        Task SaveTwitterUserApiSettingsAsync(TwitterUserApiSettings settings);
    }
}
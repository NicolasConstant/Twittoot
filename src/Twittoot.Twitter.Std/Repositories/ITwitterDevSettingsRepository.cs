using System.Threading.Tasks;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public interface ITwitterDevSettingsRepository
    {
        Task<TwitterDevApiSettings> GetTwitterDevApiSettingsAsync();
        Task SaveTwitterDevApiSettingsAsync(TwitterDevApiSettings settings);
    }
}
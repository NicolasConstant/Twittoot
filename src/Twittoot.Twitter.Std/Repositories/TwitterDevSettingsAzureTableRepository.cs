using System.Threading.Tasks;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public class TwitterDevSettingsAzureTableRepository : ITwitterDevSettingsRepository
    {
        public async Task<TwitterDevApiSettings> GetTwitterDevApiSettingsAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveTwitterDevApiSettingsAsync(TwitterDevApiSettings settings)
        {
            throw new System.NotImplementedException();
        }
    }
}
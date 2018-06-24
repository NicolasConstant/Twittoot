using System.Threading.Tasks;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public class TwitterUserSettingsAzureTableRepository : ITwitterUserSettingsRepository
    {
        public async Task<TwitterUserApiSettings> GetTwitterUserApiSettingsAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveTwitterUserApiSettingsAsync(TwitterUserApiSettings settings)
        {
            throw new System.NotImplementedException();
        }
    }
}
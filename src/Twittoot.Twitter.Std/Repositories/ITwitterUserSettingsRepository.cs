using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public interface ITwitterUserSettingsRepository
    {
        TwitterUserApiSettings GetTwitterUserApiSettings();
        void SaveTwitterUserApiSettings(TwitterUserApiSettings settings);
    }
}
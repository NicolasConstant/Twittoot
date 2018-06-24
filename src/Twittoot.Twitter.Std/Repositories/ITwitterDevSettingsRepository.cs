using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public interface ITwitterDevSettingsRepository
    {
        TwitterDevApiSettings GetTwitterDevApiSettings();
        void SaveTwitterDevApiSettings(TwitterDevApiSettings settings);
    }
}
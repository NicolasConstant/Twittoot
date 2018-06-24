using System;
using System.Threading.Tasks;
using Twittoot.Twitter.Std.Repositories;

namespace Twittoot.Twitter.Setup.Actions
{
    public class CheckIfTwitterAccountSetAction
    {
        private readonly ITwitterUserSettingsRepository _twitterUserSettingsRepository;

        #region Ctor
        public CheckIfTwitterAccountSetAction(ITwitterUserSettingsRepository twitterUserSettingsRepository)
        {
            _twitterUserSettingsRepository = twitterUserSettingsRepository;
        }
        #endregion

        public async Task<bool> ExecuteAsync()
        {
            try
            {
                var settings = await _twitterUserSettingsRepository.GetTwitterUserApiSettingsAsync();
                return settings != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Twittoot.Twitter.Std.Repositories;

namespace Twittoot.Twitter.Setup.Actions
{
    public class CheckIfTwitterApiInfoSetAction
    {
        private readonly ITwitterDevSettingsRepository _twitterDevSettingsRepository;

        #region Ctor
        public CheckIfTwitterApiInfoSetAction(ITwitterDevSettingsRepository twitterDevSettingsRepository)
        {
            _twitterDevSettingsRepository = twitterDevSettingsRepository;
        }
        #endregion

        public async Task<bool> ExecuteAsync()
        {
            try
            {
                var settings = await _twitterDevSettingsRepository.GetTwitterDevApiSettingsAsync();
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
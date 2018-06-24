using System;
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

        public bool Execute()
        {
            try
            {
                var settings = _twitterDevSettingsRepository.GetTwitterDevApiSettings();
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
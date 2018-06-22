using System;
using Twittot.Twitter.Std.Repositories;

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

        public bool Execute()
        {
            try
            {
                var settings = _twitterUserSettingsRepository.GetTwitterUserApiSettings();
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
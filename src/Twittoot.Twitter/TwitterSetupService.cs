using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;
using Twittoot.Twitter.Setup.Actions;
using Twittoot.Twitter.Setup.Dtos;
using Twittot.Twitter.Std.Repositories;

namespace Twittoot.Twitter.Setup
{
    public interface ITwitterSetupService
    {
        bool IsTwitterSet();
        void InitAndSaveTwitterAccount();
    }

    public class TwitterSetupService : ITwitterSetupService
    {
        private readonly ITwitterUserSettingsRepository _twitterUserSettingsRepository;
        private readonly ITwitterDevSettingsRepository _twitterDevSettingsRepository;

        #region Ctor
        public TwitterSetupService(ITwitterUserSettingsRepository twitterUserSettingsRepository, ITwitterDevSettingsRepository twitterDevSettingsRepository)
        {
            _twitterUserSettingsRepository = twitterUserSettingsRepository;
            _twitterDevSettingsRepository = twitterDevSettingsRepository;
        }
        #endregion
        
        public bool IsTwitterSet()
        {
            return ApiIsSet() && UserIsSet();
        }

        public void InitAndSaveTwitterAccount()
        {
            if (!ApiIsSet())
            {
                var setApiData = new GetAndSaveTwitterApiDataAction(_twitterDevSettingsRepository);
                setApiData.Execute();
            }

            if (!UserIsSet())
            {
                var setUserData = new GetAndSaveTwitterAccountDataAction(_twitterUserSettingsRepository, _twitterDevSettingsRepository);
                setUserData.Execute();
            }
        }

        private bool ApiIsSet()
        {
            var checkIfTwitterApiInfoSet = new CheckIfTwitterApiInfoSetAction(_twitterDevSettingsRepository);
            var apiSet = checkIfTwitterApiInfoSet.Execute();
            return apiSet;
        }

        private bool UserIsSet()
        {
            var checkIfTwitterAccountSet = new CheckIfTwitterAccountSetAction(_twitterUserSettingsRepository);
            var accountSet = checkIfTwitterAccountSet.Execute();
            return accountSet;
        }
    }
}

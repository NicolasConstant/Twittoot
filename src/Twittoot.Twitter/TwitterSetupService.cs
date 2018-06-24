using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;
using Twittoot.Twitter.Setup.Actions;
using Twittoot.Twitter.Setup.Dtos;
using Twittoot.Twitter.Std.Repositories;

namespace Twittoot.Twitter.Setup
{
    public interface ITwitterSetupService
    {
        Task<bool> IsTwitterSetAsync();
        Task InitAndSaveTwitterAccountAsync();
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
        
        public async Task<bool> IsTwitterSetAsync()
        {
            return await ApiIsSetAsync() && await UserIsSetAsync();
        }

        public async Task InitAndSaveTwitterAccountAsync()
        {
            if (!await ApiIsSetAsync())
            {
                var setApiData = new GetAndSaveTwitterApiDataAction(_twitterDevSettingsRepository);
                await setApiData.ExecuteAsync();
            }

            if (!await UserIsSetAsync())
            {
                var setUserData = new GetAndSaveTwitterAccountDataAction(_twitterUserSettingsRepository, _twitterDevSettingsRepository);
                await setUserData.ExecuteAsync();
            }
        }

        private async Task<bool> ApiIsSetAsync()
        {
            var checkIfTwitterApiInfoSet = new CheckIfTwitterApiInfoSetAction(_twitterDevSettingsRepository);
            var apiSet = await checkIfTwitterApiInfoSet.ExecuteAsync();
            return apiSet;
        }

        private async Task<bool> UserIsSetAsync()
        {
            var checkIfTwitterAccountSet = new CheckIfTwitterAccountSetAction(_twitterUserSettingsRepository);
            var accountSet = await checkIfTwitterAccountSet.ExecuteAsync();
            return accountSet;
        }
    }
}

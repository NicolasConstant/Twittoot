using Twittoot.Domain.BusinessRules;
using Twittoot.Domain.Models;
using Twittoot.Mastodon;
using Twittoot.Twitter;

namespace Twittoot.Domain.Factories
{
    public class ProcessAccountSyncFactory
    {
        private readonly ITwitterService _twitterService;
        private readonly IMastodonService _mastodonService;

        #region Ctor
        public ProcessAccountSyncFactory(ITwitterService twitterService, IMastodonService mastodonService)
        {
            _twitterService = twitterService;
            _mastodonService = mastodonService;
        }
        #endregion

        public ProcessAccountSynchronisation GetAccountSync(SyncAccount account)
        {
            return new ProcessAccountSynchronisation(account, _twitterService, _mastodonService);
        }
    }
}
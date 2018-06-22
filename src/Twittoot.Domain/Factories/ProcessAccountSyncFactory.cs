//using Twittoot.Domain.BusinessRules;
//using Twittoot.Domain.Models;
//using Twittoot.Domain.Repositories;
//using Twittoot.Mastodon;
//using Twittoot.Twitter;

//namespace Twittoot.Domain.Factories
//{
//    public class ProcessAccountSyncFactory
//    {
//        private readonly ITwitterService _twitterService;
//        private readonly IMastodonService _mastodonService;
//        private readonly ISyncAccountsRepository _syncAccountsRepository;

//        #region Ctor
//        public ProcessAccountSyncFactory(ITwitterService twitterService, IMastodonService mastodonService, ISyncAccountsRepository syncAccountsRepository)
//        {
//            _twitterService = twitterService;
//            _mastodonService = mastodonService;
//            _syncAccountsRepository = syncAccountsRepository;
//        }
//        #endregion

//        public ProcessAccountSynchronisation GetAccountSync(SyncAccount account)
//        {
//            return new ProcessAccountSynchronisation(account, _twitterService, _mastodonService, _syncAccountsRepository);
//        }
//    }
//}
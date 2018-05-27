using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twittoot.Domain.Models;

namespace Twittoot.Domain
{
    public class TwittootService
    {
        #region Ctor
        public TwittootService()
        {
            
        }
        #endregion

        public void RegisterNewAccount(string twitterName, string mastodonName, string mastodonInstance)
        {
            throw new NotImplementedException();
        }

        public SyncAccount[] GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
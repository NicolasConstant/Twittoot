using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Domain.Models;

namespace Twittoot.Domain.Repositories
{
    public interface ISyncAccountsRepository
    {
        SyncAccount[] GetAllAccounts();
        void SaveAccounts(SyncAccount[] accounts);
    }

    public class SyncAccountsRepository : ISyncAccountsRepository
    {
        private const string AccountsFileName = "SavedAccounts";
        private readonly string _accountsFilePath = TwittootLocation.GetUserFilePath(AccountsFileName);

        #region Ctor
        public SyncAccountsRepository()
        {
            if (!File.Exists(_accountsFilePath)) File.Create(_accountsFilePath);
        }
        #endregion

        public SyncAccount[] GetAllAccounts()
        {
            var json = File.ReadAllText(_accountsFilePath);
            return JsonConvert.DeserializeObject<SyncAccount[]>(json);
        }

        public void SaveAccounts(SyncAccount[] accounts)
        {
            var json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(_accountsFilePath, json);
        }
    }
}
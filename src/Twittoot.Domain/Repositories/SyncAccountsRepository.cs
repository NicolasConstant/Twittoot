using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Domain.Models;

namespace Twittoot.Domain.Repositories
{
    public interface ISyncAccountsRepository
    {
        SyncAccount[] GetAllAccounts();
        void UpdateAccount(SyncAccount account);
        void SaveAccounts(SyncAccount[] accounts);
    }

    public class SyncAccountsRepository : ISyncAccountsRepository
    {
        private const string AccountsFileName = "SavedAccounts.json";
        private readonly string _accountsFilePath = TwittootLocation.GetUserFilePath(AccountsFileName);

        #region Ctor
        public SyncAccountsRepository()
        {
            var json = JsonConvert.SerializeObject(new SyncAccount[0]);
            if (!File.Exists(_accountsFilePath)) File.WriteAllText(_accountsFilePath, json);
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

        public void UpdateAccount(SyncAccount account)
        {
            var allAccounts = GetAllAccounts().ToList();
            allAccounts.Remove(allAccounts.Find(x => x.Id == account.Id));
            allAccounts.Add(account);
            SaveAccounts(allAccounts.ToArray());
        }
    }
}
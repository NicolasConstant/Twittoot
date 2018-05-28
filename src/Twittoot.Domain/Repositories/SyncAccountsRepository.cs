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

        #region Ctor
        public SyncAccountsRepository()
        {
            var filePath = GetFilePath();
            if (!File.Exists(filePath)) File.Create(filePath);
        }
        #endregion

        public SyncAccount[] GetAllAccounts()
        {
            var filePath = GetFilePath();
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SyncAccount[]>(json);
        }

        public void SaveAccounts(SyncAccount[] accounts)
        {
            var filePath = GetFilePath();
            var json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(filePath, json);
        }

        private string GetFilePath()
        {
            var executingAsmDir = TwittootLocation.GetExecutingAsmLocation();
            return Path.Combine(executingAsmDir, AccountsFileName);
        }
    }
}
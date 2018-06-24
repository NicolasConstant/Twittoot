﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Domain.Sync.Models;

namespace Twittoot.Domain.Sync.Repositories
{
    public class SyncAccountsFileRepository : ISyncAccountsRepository
    {
        private const string AccountsFileName = "SavedAccounts.json";
        private readonly string _accountsFilePath = TwittootLocation.GetUserFilePath(AccountsFileName);

        #region Ctor
        public SyncAccountsFileRepository()
        {
            var json = JsonConvert.SerializeObject(new SyncAccount[0]);
            if (!File.Exists(_accountsFilePath)) File.WriteAllText(_accountsFilePath, json);
        }
        #endregion

        public async Task<SyncAccount[]> GetAllAccounts()
        {
            var json = File.ReadAllText(_accountsFilePath);
            return JsonConvert.DeserializeObject<SyncAccount[]>(json);
        }

        public async Task SaveAccounts(SyncAccount[] accounts)
        {
            var json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(_accountsFilePath, json);
        }

        public async Task UpdateAccount(SyncAccount account)
        {
            var allAccounts = (await GetAllAccounts()).ToList();
            allAccounts.Remove(allAccounts.Find(x => x.Id == account.Id));
            allAccounts.Add(account);
            await SaveAccounts(allAccounts.ToArray());
        }
    }
}
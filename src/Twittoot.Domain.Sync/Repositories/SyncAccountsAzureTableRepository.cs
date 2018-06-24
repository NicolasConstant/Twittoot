using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Twittoot.Common.Std;
using Twittoot.Domain.Sync.Models;

namespace Twittoot.Domain.Sync.Repositories
{
    public class SyncAccountsAzureTableRepository : ISyncAccountsRepository
    {
        private readonly string _storageCs;
        private readonly string _tableName;

        #region Ctor
        public SyncAccountsAzureTableRepository(string storageCs, string tableName)
        {
            _storageCs = storageCs;
            _tableName = tableName;
        }
        #endregion

        public async Task<SyncAccount[]> GetAllAccounts()
        {
            //Get table ref
            var table = await GetTable();

            //Retrieve 
            var query = new TableQuery<SyncAccountEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, AzureTableTypesStruct.SyncAccount));

            var seg = await table.ExecuteQuerySegmentedAsync<SyncAccountEntity>(query, null);
            var syncAccounts = new List<SyncAccount>();

            foreach (var result in seg.Results)
            {
                var syncAccount = new SyncAccount
                {
                    Id = result.Id,
                    TwitterName = result.TwitterName,
                    MastodonName = result.MastodonName,
                    MastodonInstance = result.MastodonInstance,
                    MastodonAccessToken = result.MastodonAccessToken,
                    LastSyncTweetId = result.LastSyncTweetId
                };
                syncAccounts.Add(syncAccount);
            }

            return syncAccounts.ToArray();
        }

        public async Task UpdateAccount(SyncAccount account)
        {
            //Get table ref
            var table = await GetTable();

            //Create entity and insert/replace
            var syncAccountEntity = new SyncAccountEntity(account);
            var insertOperation = TableOperation.InsertOrReplace(syncAccountEntity);
            await table.ExecuteAsync(insertOperation);
        }

        public async Task SaveAccounts(SyncAccount[] accounts)
        {
            //Get table ref
            var table = await GetTable();

            //Create batch
            var batchOperation = new TableBatchOperation();
            foreach (var account in accounts)
            {
                var syncAccountEntity = new SyncAccountEntity(account);
                batchOperation.InsertOrReplace(syncAccountEntity);
            }

            await table.ExecuteBatchAsync(batchOperation); //Limited for 100 entities
        }

        private async Task<CloudTable> GetTable()
        {
            // Parse the connection string and return a reference to the storage account.
            var storageAccount = CloudStorageAccount.Parse(_storageCs);

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(_tableName);

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }

    public class SyncAccountEntity : TableEntity
    {
        #region Ctor
        public SyncAccountEntity()
        {

        }

        public SyncAccountEntity(SyncAccount syncAccount)
        {
            PartitionKey = AzureTableTypesStruct.SyncAccount;
            RowKey = syncAccount.TwitterName.ToLowerInvariant();

            Id = syncAccount.Id;
            TwitterName = syncAccount.TwitterName;
            MastodonName = syncAccount.MastodonName;
            MastodonInstance = syncAccount.MastodonInstance;
            MastodonAccessToken = syncAccount.MastodonAccessToken;
            LastSyncTweetId = syncAccount.LastSyncTweetId;
        }
        #endregion

        public Guid Id { get; set; }

        public string TwitterName { get; set; }

        public string MastodonName { get; set; }

        public string MastodonInstance { get; set; }

        public string MastodonAccessToken { get; set; }

        public long LastSyncTweetId { get; set; }
    }
}
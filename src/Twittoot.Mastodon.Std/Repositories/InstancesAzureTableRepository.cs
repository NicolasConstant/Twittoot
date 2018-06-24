using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Twittoot.Common.Std;
using Twittoot.Mastodon.Std.Models;

namespace Twittoot.Mastodon.Std.Repositories
{
    public class InstancesAzureTableRepository : IInstancesRepository
    {
        private readonly string _storageCs;
        private readonly string _tableName;

        #region Ctor
        public InstancesAzureTableRepository(string storageCs, string tableName)
        {
            _storageCs = storageCs;
            _tableName = tableName;
        }
        #endregion

        public async Task<AppInfoWrapper[]> GetAllInstancesAsync()
        {
            //Get table ref
            var table = await GetTable();

            //Retrieve 
            var query = new TableQuery<AppInfoEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, AzureTableTypesStruct.Instance));

            var seg = await table.ExecuteQuerySegmentedAsync<AppInfoEntity>(query, null);
            var instances = new List<AppInfoWrapper>();

            foreach (var result in seg.Results)
            {
                var instance = new AppInfoWrapper
                {
                    id = result.Id,
                    client_id = result.ClientId,
                    client_secret = result.ClientSecret,
                    InstanceUrl = result.InstanceUrl,
                    redirect_uri = result.RedirectUri
                };
                instances.Add(instance);
            }

            return instances.ToArray();
        }

        public async Task SaveInstancesAsync(AppInfoWrapper[] instances)
        {
            //Get table ref
            var table = await GetTable();

            //Create batch
            var batchOperation = new TableBatchOperation();
            foreach (var instance in instances)
            {
                var instanceEntity = new AppInfoEntity(instance);
                batchOperation.InsertOrReplace(instanceEntity);
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

    public class AppInfoEntity : TableEntity
    {
        #region Ctor
        public AppInfoEntity()
        {

        }

        public AppInfoEntity(AppInfoWrapper appInfo)
        {
            PartitionKey = AzureTableTypesStruct.SyncAccount;
            RowKey = appInfo.InstanceUrl;

            Id = appInfo.id;
            InstanceUrl = appInfo.InstanceUrl;
            ClientId = appInfo.client_id;
            ClientSecret = appInfo.client_secret;
            RedirectUri = appInfo.redirect_uri;
        }
        #endregion

        public int Id { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RedirectUri { get; set; }

        public string InstanceUrl { get; set; }

    }
}
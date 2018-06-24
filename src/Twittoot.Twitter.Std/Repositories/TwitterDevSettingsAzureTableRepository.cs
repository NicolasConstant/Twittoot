using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Twittoot.Common.Std;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public class TwitterDevSettingsAzureTableRepository : ITwitterDevSettingsRepository
    {
        private readonly string _storageCs;
        private readonly string _tableName;

        #region Ctor
        public TwitterDevSettingsAzureTableRepository(string storageCs, string tableName)
        {
            _storageCs = storageCs;
            _tableName = tableName;
        }
        #endregion

        public async Task<TwitterDevApiSettings> GetTwitterDevApiSettingsAsync()
        {
            //Get table ref
            var table = await GetTable();

            //Retrieve 
            var query = new TableQuery<TwitterDevApiSettingsEntity>()
                .Where(
                    TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, AzureTableTypesStruct.Twitter),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, TwitterSettingsStruct.TwitterApi)));

            var seg = await table.ExecuteQuerySegmentedAsync<TwitterDevApiSettingsEntity>(query, null);

            var result = seg.Results.FirstOrDefault();
            if (result == default(TwitterDevApiSettingsEntity)) return null;

            var setting = new TwitterDevApiSettings
            {
                ConsumerKey = result.ConsumerKey,
                ConsumerSecret = result.ConsumerSecret
            };

            return setting;
        }

        public async Task SaveTwitterDevApiSettingsAsync(TwitterDevApiSettings settings)
        {
            //Get table ref
            var table = await GetTable();

            //Create entity and insert/replace
            var devSettings = new TwitterDevApiSettingsEntity(settings);
            var insertOperation = TableOperation.InsertOrReplace(devSettings);
            await table.ExecuteAsync(insertOperation);
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


    public class TwitterDevApiSettingsEntity : TableEntity
    {
        #region Ctor
        public TwitterDevApiSettingsEntity()
        {

        }

        public TwitterDevApiSettingsEntity(TwitterDevApiSettings apiSettings)
        {
            PartitionKey = AzureTableTypesStruct.Twitter;
            RowKey = TwitterSettingsStruct.TwitterApi;


            ConsumerKey = apiSettings.ConsumerKey;
            ConsumerSecret = apiSettings.ConsumerSecret;
        }
        #endregion

        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }
    }
}
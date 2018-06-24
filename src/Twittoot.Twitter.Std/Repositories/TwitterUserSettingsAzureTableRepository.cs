using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Twittoot.Common.Std;
using Twittoot.Twitter.Setup.Settings;

namespace Twittoot.Twitter.Std.Repositories
{
    public class TwitterUserSettingsAzureTableRepository : ITwitterUserSettingsRepository
    {
        private readonly string _storageCs;
        private readonly string _tableName;

        #region Ctor
        public TwitterUserSettingsAzureTableRepository(string storageCs, string tableName)
        {
            _storageCs = storageCs;
            _tableName = tableName;
        }
        #endregion

        public async Task<TwitterUserApiSettings> GetTwitterUserApiSettingsAsync()
        {
            //Get table ref
            var table = await GetTable();

            //Retrieve 
            var query = new TableQuery<TwitterUserApiSettingsEntity>()
                .Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, AzureTableTypesStruct.Twitter),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, TwitterSettingsStruct.TwitterAccount)));

            var seg = await table.ExecuteQuerySegmentedAsync<TwitterUserApiSettingsEntity>(query, null);

            var result = seg.Results.FirstOrDefault();
            if (result == default(TwitterUserApiSettingsEntity)) return null;

            var setting = new TwitterUserApiSettings
            {
                AccessToken = result.AccessToken,
                AccessTokenSecret = result.AccessTokenSecret
            };

            return setting;
        }

        public async Task SaveTwitterUserApiSettingsAsync(TwitterUserApiSettings settings)
        {
            //Get table ref
            var table = await GetTable();

            //Create entity and insert/replace
            var devSettings = new TwitterUserApiSettingsEntity(settings);
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

    public class TwitterUserApiSettingsEntity : TableEntity
    {
        #region Ctor
        public TwitterUserApiSettingsEntity()
        {

        }

        public TwitterUserApiSettingsEntity(TwitterUserApiSettings apiSettings)
        {
            PartitionKey = AzureTableTypesStruct.Twitter;
            RowKey = TwitterSettingsStruct.TwitterAccount;

            AccessToken = apiSettings.AccessToken;
            AccessTokenSecret = apiSettings.AccessTokenSecret;
        }
        #endregion

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }
    }
}
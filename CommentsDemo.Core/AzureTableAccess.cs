using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CommentsDemo.Common;
using CommentsDemo.Core.AzureModel;

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace CommentsDemo.Core
{
    internal class AzureTableAccess : IAzureTableAccess
    {
        public AzureTableAccess()
        {
            AddSecret();

            this.connectionString = GetConnectionString();
        }

        public async Task<CloudTable> CreateTableAsync(string tableName)
        {
            CloudTableClient tableClient = CreateTableClient(this.connectionString);
            CloudTable table = tableClient.GetTableReference(tableName);

            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Created Table named: {0}", tableName);
            }
            else
            {
                Console.WriteLine("Table {0} already exists", tableName);
            }

            return table;
        }

        // TODO Insert with TableBatchOperation in order to generate a lot more data
        public async Task<CommentEntity> InsertCommentAsync(string tableName, string productName, string comment)
        {
            try
            {
                CloudTableClient tableClient = CreateTableClient(this.connectionString);
                CloudTable table = tableClient.GetTableReference(tableName);

                TableOperation insertOperation = TableOperation.Insert(new CommentEntity(productName, comment));
                TableResult result = await table.ExecuteAsync(insertOperation);
                CommentEntity insertedComment = result.Result as CommentEntity;

                double? charge = result.RequestCharge.HasValue ? result.RequestCharge : 0;
                Console.WriteLine($"RC_log_{charge}_{MethodBase.GetCurrentMethod().Name}_{productName}");

                return insertedComment;
            }
            catch (StorageException e)
            {
                throw new CommentsDemoException("Exception occured during comment insert call.", e);
            }
        }

        public async Task<IEnumerable<CommentEntity>> RetrieveCommentsAsync(string tableName, string productName)
        {
            try
            {
                List<CommentEntity> result = new List<CommentEntity>();
                TableContinuationToken continuationToken = null;

                CloudTableClient tableClient = CreateTableClient(this.connectionString);
                CloudTable table = tableClient.GetTableReference(tableName);

                string wherePart = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, productName);
                TableQuery<CommentEntity> query = new TableQuery<CommentEntity> { TakeCount = 1000 }.Where(wherePart);

                do
                {
                    TableQuerySegment<CommentEntity> segment = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                    result.AddRange(segment.Results);

                    continuationToken = segment.ContinuationToken;
                }
                while (continuationToken != null);


                return result;
            }
            catch (StorageException e)
            {
                throw new CommentsDemoException("Exception occured during comment insert call.", e);
            }
        }

        public async Task<IEnumerable<string>> RetrieveProductListAsync(string tableName)
        {
            try
            {
                List<string> output = new List<string>();
                TableContinuationToken continuationToken = null;

                CloudTableClient tableClient = CreateTableClient(this.connectionString);
                CloudTable table = tableClient.GetTableReference(tableName);
                // Maximum TakeCount is 1000
                TableQuery tableQuery = new TableQuery { TakeCount = 1000 }.Select(new List<string> { "PartitionKey" });

                do
                {
                    TableQuerySegment<DynamicTableEntity> segment = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                    output.AddRange(segment.Results.Select(dynRes => dynRes.PartitionKey).Distinct());

                    continuationToken = segment.ContinuationToken;
                }
                while (continuationToken != null);

                return output.Distinct();
            }

            catch (StorageException e)
            {
                throw new CommentsDemoException($"Exception occured during {MethodBase.GetCurrentMethod().Name} call.", e);
            }
        }

        private static void AddSecret()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
            {
                env = "Development";
            }

            if (env == "Development")
            {
                builder.AddUserSecrets<AzureTableAccess>();
            }
            configuration = builder.Build();
        }

        private static string GetConnectionString()
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionStringForConsole");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new CommentsDemoException($"Missing DefaultConnectionStringForConsole information!");
            }

            return connectionString;
        }

        private static CloudTableClient CreateTableClient(string connStringIn)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connStringIn);
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            return tableClient;
        }

        private readonly string connectionString;
        private static IConfigurationRoot configuration;
    }
}

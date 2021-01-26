using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using CommentsDemo.Common;

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace CommentsDemo.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Storage Table");

            AddSecret();

            string connString = GetConnectionString();

            //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connString);
            //CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient(
            //    new TableClientConfiguration() {UseRestExecutorForCosmosEndpoint = true});

            //List<CloudTable> tables = tableClient.ListTables().ToList();

            //if (!tables.Any((p) => p.Name.Equals(tableName)))
            //{
            //    throw new CommentsDemoException($"Missing CloudTable: {tableName} !");
            //}

            Console.WriteLine("Hello CommentsDemo console user!");
            DataAccess dataAccess = new DataAccess();

            dataAccess.FillWithDemoData(5,5);
            ProductDTO[] result = dataAccess.GetProducts().ToArray();

            foreach (ProductDTO item in result)
            {
                string jsonString = JsonSerializer.Serialize(item);
                Console.WriteLine($"Product information:");
                Console.Write($"{jsonString}");
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
                builder.AddUserSecrets<Program>();
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

        private const string tableName = "commentDemoTable0";
        private static IConfigurationRoot configuration;
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;

namespace TableStorage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=leedaleteststorage;AccountKey=//S1ohrGGFaDrIp+iIjGWzMwHTXN6hvESIi37AGiUnBiOm+4sgTCC0mNe8mwmpGD02fQ5PTF2UMhe7u2af2oVg==;EndpointSuffix=core.windows.net");
            CloudTable table = await CreateTable(account);
            var entity = await InsertOrMergeEntityAsync(table, new CustomerEntity("Dale", "Lee") { Email = "lee.jdale@gmail.com" });

            string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Dale");
            var results = GetQueryResults(table, filter);

            foreach (CustomerEntity customer in results)
            {
                Console.WriteLine($"Customer: {entity.Email}");
            }

            Console.ReadLine();
        }

        static IEnumerable<CustomerEntity> GetQueryResults(CloudTable table, string filter)
        {
            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(filter);
            return table.ExecuteQuery(query);
        }

        static async Task<CloudTable> CreateTable(CloudStorageAccount account)
        {
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("LeeTestTable");

            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Table created.");
            }

            return table;
        }
        
        static async Task<CustomerEntity> InsertOrMergeEntityAsync(CloudTable table, CustomerEntity entity)
        {
            TableOperation insertOrMergeOp = TableOperation.InsertOrMerge(entity);
            TableResult result = await table.ExecuteAsync(insertOrMergeOp);
            return result.Result as CustomerEntity;
        }
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {
        }

        public CustomerEntity(string lastname, string firstname)
        {
            PartitionKey = lastname;
            RowKey = firstname;    
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}

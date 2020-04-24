using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchServiceName = "lee-test-search";
            string indexName = "test-index";

            SearchServiceClient searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials("B4EE41740C65A72C6A96909BBDD64F52"));

            Console.WriteLine("Creating index...");
            CreateIndex(indexName, searchClient);

            SearchIndexClient indexClient = new SearchIndexClient(searchServiceName, indexName, new SearchCredentials("B4EE41740C65A72C6A96909BBDD64F52"));

            Console.WriteLine("Creating index...");
            UploadDocuments(indexClient);
        }

        static void CreateIndex(string indexName, SearchServiceClient serviceClient)
        {
            Microsoft.Azure.Search.Models.Index index = new Microsoft.Azure.Search.Models.Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<Customer>()
            };

            serviceClient.Indexes.Create(index);
        }

        static void UploadDocuments(ISearchIndexClient indexClient)
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer() { Name = "Tom"},
                new Customer() { Name = "John"},
                new Customer() { Name = "Bob"},
                new Customer() { Name = "Jim"}
            };

            IndexBatch<Customer> batch = IndexBatch.Upload(customers);
            indexClient.Documents.Index(batch);
        }
    }

    class Customer
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Name { get; set; }
    }
}

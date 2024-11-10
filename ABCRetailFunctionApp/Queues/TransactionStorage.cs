using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System.Configuration;
using Azure.Storage.Files.Shares.Models;
using System;

namespace ABCRetailFunctionApp.Queues
{
    public class TransactionStorage : QueueStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionStorage() :
            base("DefaultEndpointsProtocol=https;AccountName=cldv6212st10069070;AccountKey=jBL87x/bYiGzKjsEyu4SfL1JdJ7Jb+SXTqzXsd/GwBh0qvX8sEUAp9LuBDCbsDjfJL7CY3S8esV9+AStSk/Srg==;EndpointSuffix=core.windows.net",
                "transactions") { }
    }
}

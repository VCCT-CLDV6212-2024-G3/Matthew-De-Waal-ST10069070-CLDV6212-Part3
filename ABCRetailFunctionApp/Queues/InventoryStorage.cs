using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System.Configuration;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Queues.Models;
using System;

namespace ABCRetailFunctionApp.Queues
{
    public class InventoryStorage : QueueStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryStorage() :
            base("DefaultEndpointsProtocol=https;AccountName=cldv6212st10069070;AccountKey=jBL87x/bYiGzKjsEyu4SfL1JdJ7Jb+SXTqzXsd/GwBh0qvX8sEUAp9LuBDCbsDjfJL7CY3S8esV9+AStSk/Srg==;EndpointSuffix=core.windows.net",
                "inventory") { }
    }
}

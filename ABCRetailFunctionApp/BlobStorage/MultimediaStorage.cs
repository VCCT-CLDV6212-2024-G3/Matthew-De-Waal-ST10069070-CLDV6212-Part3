using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files;
using System.Configuration;
using System.IO;
using System.Configuration.Internal;
using System;

namespace ABCRetailFunctionApp.BlobStorage
{
    public class MultimediaStorage : BlobStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MultimediaStorage() : 
            base("DefaultEndpointsProtocol=https;AccountName=cldv6212st10069070;AccountKey=jBL87x/bYiGzKjsEyu4SfL1JdJ7Jb+SXTqzXsd/GwBh0qvX8sEUAp9LuBDCbsDjfJL7CY3S8esV9+AStSk/Srg==;EndpointSuffix=core.windows.net",
                "multimedia") { }
    }
}

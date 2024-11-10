using Azure.Storage.Blobs;
using System;

namespace ABCRetailFunctionApp.BlobStorage
{
    public class ProductImageStorage : BlobStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductImageStorage() : 
            base("DefaultEndpointsProtocol=https;AccountName=cldv6212st10069070;AccountKey=jBL87x/bYiGzKjsEyu4SfL1JdJ7Jb+SXTqzXsd/GwBh0qvX8sEUAp9LuBDCbsDjfJL7CY3S8esV9+AStSk/Srg==;EndpointSuffix=core.windows.net",
                "product-images") { }
    }
}

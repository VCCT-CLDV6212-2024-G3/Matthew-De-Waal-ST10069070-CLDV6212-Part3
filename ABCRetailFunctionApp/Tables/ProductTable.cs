using Azure.Data.Tables;
using System;

namespace ABCRetailFunctionApp.Tables
{
    public class ProductTable : TableStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductTable() : 
            base("DefaultEndpointsProtocol=https;AccountName=cldv6212st10069070;AccountKey=jBL87x/bYiGzKjsEyu4SfL1JdJ7Jb+SXTqzXsd/GwBh0qvX8sEUAp9LuBDCbsDjfJL7CY3S8esV9+AStSk/Srg==;EndpointSuffix=core.windows.net",
                "Product", 
                new string[] {"Id", "ProductName", "Price", "Category", "TotalStock", "ImagePath"}) { }
    }
}

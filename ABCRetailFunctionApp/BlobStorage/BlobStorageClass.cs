
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.IO;
using System;

namespace ABCRetailFunctionApp.BlobStorage
{
    public abstract class BlobStorageClass
    {
        // Data fields
        private readonly string connectionString;
        private readonly string containerName;

        /// <summary>
        /// Master constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="containerName"></param>
        public BlobStorageClass(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }

        /// <summary>
        /// Indexer to get and set the stream of a specific blob.
        /// </summary>
        /// <param name="name">The name of the blob</param>
        /// <returns>The blob stream</returns>
        public Stream this[string name]
        {
            get
            {
                // Declare and instantiate a BlobClient object.
                BlobClient client = new BlobClient(connectionString, containerName, name);

                return client.DownloadContentAsync().Result.Value.Content.ToStream();
            }
            set
            {
                // Declare and instantiate a BlobClient object.
                BlobClient client = new BlobClient(connectionString, containerName, name);
                // Upload the data to the blob.
                client.UploadAsync(value);
            }
        }

        /// <summary>
        /// Adds a new blob item to the blob container.
        /// </summary>
        /// <param name="name">The name of the blob item</param>
        /// <param name="bytes">The data (in bytes) of the blob.</param>
        public void AddItem(string name, byte[] bytes)
        {
            // Declare and instantiate a BlobClient object.
            BlobClient client = new BlobClient(connectionString, containerName, name);
            // Declare and instantiate a BinaryData object.
            BinaryData binaryData = new BinaryData(bytes);
            // Upload the binary data to the blob.
            client.UploadAsync(binaryData);
        }

        /// <summary>
        /// Deletes an existing blob item from the blob container.
        /// </summary>
        /// <param name="name">The name of the blob item</param>
        public void DeleteItem(string name)
        {
            // Declare and instantiate a BlobClient object.
            BlobClient client = new BlobClient(connectionString, containerName, name);
            // Delete the blob item.
            client.DeleteAsync();
        }

        /// <summary>
        /// Deletes all the blob items in the blob container.
        /// </summary>
        public void DeleteAll()
        {
            // Declare and instantiate a BlobContainerClient object.
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);
            var blobs = client.GetBlobs();

            // Iterate through the 'blobs' collection.
            foreach(BlobItem item in blobs)
            {
                // Delete the blob item.
                client.DeleteBlob(item.Name);
            }
        }

        /// <summary>
        /// Returns a list of all the blob names in the blob container.
        /// </summary>
        /// <returns></returns>
        public StringCollection RecieveData(bool bFullPath)
        {
            // Declare and instantiate a BlobContainerClient object.
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);
            var blobs = client.GetBlobs();
            StringCollection collection = new StringCollection();

            // Iterate through the blobs collection.
            foreach(BlobItem item in blobs)
            {
                if (!bFullPath)
                {
                    // Add the blob item.
                    collection.Add(item.Name);
                }
                else
                {
                    // Add the blob item.
                    collection.Add(GetFullPath(item.Name));
                }
            }

            return collection;
        }

        /// <summary>
        /// Returns a list of all the blob names in a specific directory of the blob container.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public StringCollection RecieveData(string path, bool bFullPath)
        {
            // Declare and instantiate a BlobContainerClient object.
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);
            var blobs = client.GetBlobs();
            StringCollection collection = new StringCollection();

            // Iterate through the blobs collection.
            foreach (BlobItem item in blobs)
            {
                // Check if the blob item is found in the directory.
                if ($"/{item.Name}/".Contains(path))
                {
                    if (!bFullPath)
                    {
                        // Add the blob item.
                        collection.Add(item.Name);
                    }
                    else
                    {
                        // Add the blob item.
                        collection.Add(GetFullPath(item.Name));
                    }
                }
            }

            return collection;
        }

        /// <summary>
        /// Gets the full path of a blob item.
        /// </summary>
        /// <param name="name">The name of the blob item</param>
        /// <returns></returns>
        private string GetFullPath(string name)
        {
            // Declare and instantiate a BlobContainerClient object.
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);

            return $"{client.Uri.ToString()}/{name}";
        }
    }
}

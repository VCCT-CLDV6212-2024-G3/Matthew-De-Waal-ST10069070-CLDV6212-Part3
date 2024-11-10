using ABCRetailFunctionApp.BlobStorage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ABCRetailFunctionApp
{
    public class WriteToBlobStorage
    {
        private readonly ILogger _logger;

        public WriteToBlobStorage(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WriteToBlobStorage>();
        }

        [Function("WriteToBlobStorage")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            // Obtain the url parameter values.
            string blobContainer = req.Query["container"];
            string filename = req.Query["filename"];

            byte[] bytes = new byte[req.Body.Length];
            req.Body.Read(bytes, 0, bytes.Length);

            if (blobContainer == "MULTIMEDIA")
            {
                // Declare and instantiate a MultimediaSrorage object.
                MultimediaStorage storage = new MultimediaStorage();
                storage.AddItem(filename, bytes);
            }

            if (blobContainer == "PRODUCT-IMAGES")
            {
                // Declare and instantiate a ProductImageStorage object.
                ProductImageStorage storage = new ProductImageStorage();
                storage.AddItem(filename, bytes);
            }

            return null;
        }
    }
}

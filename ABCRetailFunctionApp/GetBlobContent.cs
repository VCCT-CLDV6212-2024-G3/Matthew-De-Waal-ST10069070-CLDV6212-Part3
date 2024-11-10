using ABCRetailFunctionApp.BlobStorage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json.Serialization;

namespace ABCRetailFunctionApp
{
    public class GetBlobContent
    {
        private readonly ILogger _logger;

        public GetBlobContent(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetBlobContent>();
        }

        [Function("GetBlobContent")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Obtain the container from the query.
            string container = req.Query["container"];
            // Declare a StringCollection object and initialize it to null.
            StringCollection result = null;

            if (container == "MULTIMEDIA")
            {
                // Declare and instantiate a MultimediaStorage object.
                MultimediaStorage storage = new MultimediaStorage();

                if (req.Query["path"] != null)
                    result = storage.RecieveData(req.Query["path"], true);
                else
                    result = storage.RecieveData(true);
            }

            if (container == "PRODUCT-IMAGE")
            {
                // Declare and instantiate a ProductImageStorage object.
                ProductImageStorage storage = new ProductImageStorage();

                if (req.Query["path"] != null)
                    result = storage.RecieveData(req.Query["path"], true);
                else
                    result = storage.RecieveData(true);
            }

            // Declare and instantiate a string array.
            string[] items = new string[result.Count];
            result.CopyTo(items, 0);

            // Convert the object 'items' to a JSON string.
            string responseData = JsonConvert.SerializeObject(items);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteStringAsync(responseData);

            return response;
        }
    }
}

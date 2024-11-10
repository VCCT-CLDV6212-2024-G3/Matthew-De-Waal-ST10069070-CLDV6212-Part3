using ABCRetailFunctionApp.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace ABCRetailFunctionApp
{
    public class GetProductDetails
    {
        private readonly ILogger _logger;

        public GetProductDetails(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProductDetails>();
        }

        [Function("GetProductDetails")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Obtain the productId from the query.
            string productId = req.Query["ProductId"];

            // Declare and instantiate a ProductTable object.
            ProductTable table = new ProductTable();
            // Declare a string array.
            string[] items = null;

            // Iterate through the collection 'table'.
            foreach(var item in table)
            {
                if (item[0] == productId)
                {
                    // Instantiate the string array.
                    items = new string[item.Count];
                    item.CopyTo(items, 0);
                    // Exit the loop.
                    break;
                }
            }

            // Create a response object.
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteStringAsync(JsonConvert.SerializeObject(items));

            return response;
        }
    }
}

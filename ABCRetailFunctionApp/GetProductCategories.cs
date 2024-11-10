using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;

namespace ABCRetailFunctionApp
{
    public class GetProductCategories
    {
        private readonly ILogger _logger;

        public GetProductCategories(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProductCategories>();
        }

        [Function("GetProductCategories")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Create a response object.
            var response = req.CreateResponse(HttpStatusCode.OK);
            string[] categories = { "Chairs", "Couches", "Stoves", "Tables", "Televisions", "Washing Machines" };

            response.WriteStringAsync(JsonConvert.SerializeObject(categories));

            return response;
        }
    }
}

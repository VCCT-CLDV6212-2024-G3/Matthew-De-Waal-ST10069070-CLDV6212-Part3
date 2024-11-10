using ABCRetailFunctionApp.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;

namespace ABCRetailFunctionApp
{
    public class SendFeedback
    {
        private readonly ILogger _logger;

        public SendFeedback(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SendFeedback>();
        }

        [Function("SendFeedback")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            // Get the message content from the request body.
            string message = new StreamReader(req.Body).ReadToEndAsync().Result;

            // Declare and instantiate an InventoryStorage object.
            InventoryStorage inventoryStorage = new InventoryStorage();
            // Add the message to the inventoryStorage.
            inventoryStorage.AddMessage(message);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}

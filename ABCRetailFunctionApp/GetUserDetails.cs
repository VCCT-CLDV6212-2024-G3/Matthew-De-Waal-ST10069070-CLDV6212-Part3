using ABCRetailFunctionApp.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace ABCRetailFunctionApp
{
    public class GetUserDetails
    {
        private readonly ILogger _logger;

        public GetUserDetails(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetUserDetails>();
        }

        [Function("GetUserDetails")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Obtain the base64 username and password from the query.
            string base64_userName = req.Query["username"];
            string base64_password = req.Query["password"];

            string userName = Encoding.UTF8.GetString(Convert.FromBase64String(base64_userName));
            string password = Encoding.UTF8.GetString(Convert.FromBase64String(base64_password));
            bool userAccountExists = false;
            string userPassword = string.Empty;

            CustomerProfileTable table = new CustomerProfileTable();
            StringCollection record2 = null;

            // Iterate through the collection 'table'.
            foreach(var record in table)
            {
                if (record[0] == userName)
                {
                    userAccountExists = true;
                    userPassword = record[3];
                    record2 = record;
                    break;
                }
            }

            // Create a response object.
            var response = req.CreateResponse(HttpStatusCode.OK);

            if(userAccountExists && password == userPassword)
            {
                dynamic userDetails = new { UserName = userName, FirstName = record2[1], LastName = record2[2] };
                response.WriteStringAsync(JsonConvert.SerializeObject((object)userDetails));
            }
            else
            {
                response.WriteStringAsync("Failed");
            }

            return response;
        }
    }
}

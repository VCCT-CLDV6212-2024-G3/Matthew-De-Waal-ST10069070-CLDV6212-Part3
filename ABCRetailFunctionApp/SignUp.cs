using ABCRetailFunctionApp.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace ABCRetailFunctionApp
{
    public class SignUp
    {
        private readonly ILogger _logger;

        public SignUp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SignUp>();
        }

        [Function("SignUp")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            // Read the content of the request body.
            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            dynamic registrationInfo = JsonConvert.DeserializeObject(requestBody);

            // Obtain the user details.
            string userName = Convert.ToString(registrationInfo.UserName);
            string firstName = Convert.ToString(registrationInfo.FirstName);
            string lastName = Convert.ToString(registrationInfo.LastName);
            string password = Convert.ToString(registrationInfo.Password);

            // Declare and instantiate a CustomerProfileTable object.
            CustomerProfileTable table = new CustomerProfileTable();
            bool userAccountExists = false;

            // Iterate through the collection 'table'.
            foreach(var record in table)
            {
                if (record[0] == userName)
                {
                    userAccountExists = true;
                    break;
                }
            }

            if(!userAccountExists)
            {
                // Add the record if the user account does not exist.
                table.AddRecord(new string[] { userName, firstName, lastName, password });

                return req.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}

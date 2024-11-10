using ABCRetailFunctionApp.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace ABCRetailFunctionApp
{
    public class SignIn
    {
        private readonly ILogger _logger;

        public SignIn(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SignIn>();
        }

        [Function("SignIn")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            // Obtain the user login info from the request body.
            string sUserLoginInfo = new StreamReader(req.Body).ReadToEndAsync().Result;
            dynamic userLoginInfo = JsonConvert.DeserializeObject(sUserLoginInfo);

            // Get the provided username and password.
            string userName = Convert.ToString(userLoginInfo.UserName);
            string password = Convert.ToString(userLoginInfo.Password);

            bool userAccountExists = false;
            string userPassword = string.Empty;
            CustomerProfileTable table = new CustomerProfileTable();
            StringCollection record2 = null;

            // Iterate through the collection 'table'.
            foreach(StringCollection record in table)
            {
                if (record[0] == userName)
                {
                    userAccountExists = true;
                    userPassword = record[3];
                    record2 = record;
                }
            }

            // Create a response object.
            var response = req.CreateResponse(HttpStatusCode.OK);

            if (userAccountExists && password == userPassword)
            {
                response.Headers.Add("UserSignIn", "true");
            }
            else
            {
                response.Headers.Add("UserSignIn", "false");
            }

            return response;
        }
    }
}

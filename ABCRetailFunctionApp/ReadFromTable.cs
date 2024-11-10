using ABCRetailFunctionApp.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace ABCRetailFunctionApp
{
    public class ReadFromTable
    {
        private readonly ILogger _logger;

        public ReadFromTable(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ReadFromTable>();
        }

        [Function("ReadFromTable")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Obtain the query values.
            string container = req.Query["container"];
            string sRecordIndex = req.Query["recordIndex"];
            string propertyName = req.Query["propertyName"];
            object result = null;

            if(container == "CUSTOMER-PROFILE" || container == "PRODUCT")
            {
                // Declare a TableStorageClass object.
                TableStorageClass table = null;

                if (container == "CUSTOMER-PROFILE")
                    table = new CustomerProfileTable();

                if (container == "PRODUCT")
                    table = new ProductTable();

                if (sRecordIndex != null)
                {
                    int recordIndex = Convert.ToInt32(sRecordIndex);

                    if (propertyName == null)
                    {
                        var record = table[recordIndex];
                        string[] items = new string[record.Count];
                        record.CopyTo(items, 0);

                        result = items;
                    }
                    else
                    {
                        result = table[recordIndex, propertyName];
                    }
                }
                else
                {
                    // Declare and instantiate a string list object.
                    List<string[]> items = new List<string[]>();

                    foreach(var record in table)
                    {
                        string[] item = new string[record.Count];
                        record.CopyTo(item, 0);

                        items.Add(item);
                    }

                    result = items.ToArray();
                }
            }
            
            // Create a response object.
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteStringAsync(JsonConvert.SerializeObject(result));

            return response;
        }
    }
}
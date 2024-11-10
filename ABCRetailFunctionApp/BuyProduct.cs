using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Net;
using System.Text;
using ABCRetailFunctionApp.FileShares;
using ABCRetailFunctionApp.Queues;

namespace ABCRetailFunctionApp
{
    public class BuyProduct
    {
        private readonly ILogger _logger;

        public BuyProduct(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BuyProduct>();
        }

        [Function("BuyProduct")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            // Obtain the data from the request.
            string sRequestData = new StreamReader(req.Body).ReadToEndAsync().Result;
            // Create a dynamic variable from sRequestData.
            dynamic requestData = JsonConvert.DeserializeObject(sRequestData);

            // Obtain the data from the dynamic variable.
            string productId = Convert.ToString(requestData?.ProductId);
            string userName = Convert.ToString(requestData?.UserName);
            string mode = Convert.ToString(requestData?.Mode);

            // Declare and instantaite a StringBuilder object.
            StringBuilder sb = new StringBuilder();

            if (mode == "OnceOff")
            {
                // Build the report for a once-off payment.
                sb.AppendLine("ONCE OFF PURCHASE");
                sb.AppendLine($"Product ID: {productId}");
                sb.AppendLine($"UserName: {userName}");
                sb.AppendLine($"Mode: {mode}");

                // Convert the report to a collection of bytes.
                byte[] contentBytes = Encoding.UTF8.GetBytes(sb.ToString());
                // Generate a reciept filename.
                string receiptFileName = $"receipt-{Guid.NewGuid()}.txt";

                LogStorage logStorage = new LogStorage();
                // Add the report to the LogStorage of ABC Retail.
                logStorage.AddItem(receiptFileName, contentBytes);

                TransactionStorage transactionStorage = new TransactionStorage();
                // Add a notification message to the TransactionStorage of ABC Retail.
                transactionStorage.AddMessage($"PRODUCT PURCHASED : {receiptFileName}");
            }

            if (mode == "Contract")
            {
                // Build the report for a two year contract payment.
                sb.AppendLine("TWO YEAR CONTRACT");
                sb.AppendLine($"Product ID: {productId}");
                sb.AppendLine($"UserName: {userName}");
                sb.AppendLine($"Mode: {mode}");

                // Define the duration of the contract.
                DateTime contractBeginDate = DateTime.Now;
                DateTime contractEndDate = DateTime.Now.AddYears(2);

                sb.AppendLine($"Begin Date: {contractBeginDate.ToString("yyyy-MM-dd")}");
                sb.AppendLine($"End Date: {contractEndDate.ToString("yyyy-MM-dd")}");

                // Convert the report to a collection of bytes.
                byte[] contentBytes = Encoding.UTF8.GetBytes(sb.ToString());
                // Generate a contract filename.
                string contractFileName = $"contract-{Guid.NewGuid()}.txt";

                ContractStorage contractStorage = new ContractStorage();
                // Add the report to the ContractStorage of ABC Retail.
                contractStorage.AddItem(contractFileName, contentBytes);

                TransactionStorage transactionStorage = new TransactionStorage();
                // Add a notification message to the TransactionStorage of ABC Retail.
                transactionStorage.AddMessage($"PRODUCT PURCHASED : {contractFileName}");
            }

            // The request succeeded.
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}

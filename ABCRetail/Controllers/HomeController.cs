using ABCRetail.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace ABCRetail.Controllers
{
    using ABCRetail.Models;

    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration)
        {

        }

        [HttpGet]
        public IActionResult GetData()
        {
            // Get the url from the request header.
            string url = this.Request.Headers["URL"].ToString();

            // Declare and instantiate a HttpClient object.
            HttpClient client = new HttpClient();
            // Read the response from the client.
            var response = client.GetAsync(url).Result;

            // Write the response to the Http Response object.
            this.Response.WriteAsync(response.Content.ReadAsStringAsync().Result);
            this.Response.CompleteAsync();

            return View();
        }

        [HttpPost]
        public IActionResult PostData()
        {
            // Get the url from the request header.
            string url = this.Request.Headers["URL"].ToString();
            // Get the request body.
            string requestBody = new StreamReader(this.Request.Body).ReadToEndAsync().Result;

            // Declare and instantiate a HttpClient object.
            HttpClient client = new HttpClient();
            // Declare and instantiate a StringContent object.
            StringContent content = new StringContent(requestBody);
            // Post the content to the url asynchronously.
            var clientResult = client.PostAsync(url, content);

            // Iterate through the header collection.
            foreach(var item in clientResult.Result.Headers)
            {
                this.Response.Headers.Add(item.Key, item.Value.ToList()[0]);
            }

            return View();
        }

        [HttpPost]
        public IActionResult SetUserDetails()
        {
            // Obtain the request body.
            string requestBody = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
            // Construct an object using JsonConvert.
            dynamic userDetails = JsonConvert.DeserializeObject(requestBody);

            // Assign the user details.
            ABCRetail.UserName = Convert.ToString(userDetails.UserName);
            ABCRetail.UserFirstName = Convert.ToString(userDetails.FirstName);
            ABCRetail.UserLastName = Convert.ToString(userDetails.LastName);
            ABCRetail.UserLoggedIn = true;

            return View();
        }

        public IActionResult Index()
        {
            // Check if the request contains a 'SignOut' argument.
            if (this.Request.Query["Action"] == "SignOut")
            {
                // Sign-out the user.
                ABCRetail.UserLoggedIn = false;
                ABCRetail.UserName = string.Empty;
                ABCRetail.UserFirstName = string.Empty;
                ABCRetail.UserLastName = string.Empty;
            }

            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult ProductDisplay()
        {
            return View();
        }

        public IActionResult BuyProduct()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

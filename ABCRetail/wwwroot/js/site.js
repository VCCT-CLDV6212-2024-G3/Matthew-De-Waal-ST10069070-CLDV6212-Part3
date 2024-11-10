// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Redirects the user to the homepage.
function RedirectToHomePage() {
    window.location = "/Home/Index";
}

// Registers a user with ABC Retail.
function SignUp(userName, firstName, lastName, password, confirmedPassword) {

    // Perform validation on the parameters.
    let validation = userName.length > 0 &&
        firstName.length > 0 &&
        lastName.length > 0 &&
        password.length >= 8 &&
        password == confirmedPassword;

    // Continue if the validation succeeds.
    if (validation) {
        let registration = { "UserName": userName, "FirstName": firstName, "LastName": lastName, "Password": password };

        POST_DATA("https://abcretail-st10069070.azurewebsites.net/api/SignUp", JSON.stringify(registration), function (request) {
            if (request.status == 200) {
                // The request succeeded/
                window.location = "/Home/SignIn"
            } else {
                // The request failed.
                document.getElementById("content1").style.display = "none";
                document.getElementById("content2").style.display = "block";
            }
        });
    }
    else {
        // The validation failed.
        document.getElementById("content1").style.display = "none";
        document.getElementById("content2").style.display = "block";
    }
}

// Attempts to sign in a user with the provided credentials.
function SignIn(userName, password) {
    // Perform validation on the parameters.
    let validation = userName.length > 0 && password.length > 0;

    // Continue if the validation succeeds.
    if (validation) {
        let signInRequest = { "UserName": userName, "Password": password };

        POST_DATA("https://abcretail-st10069070.azurewebsites.net/api/SignIn", JSON.stringify(signInRequest), function (request) {
            let bSuccess = request.getResponseHeader("UserSignIn");

            if (bSuccess == "true") {

                GET_DATA("https://abcretail-st10069070.azurewebsites.net/api/GetUserDetails?username=" + btoa(userName) + "&password=" + btoa(password), function (status, response) {
                    if (response != "Failed") {
                        let request = new XMLHttpRequest();
                        request.open("POST", "/Home/SetUserDetails", true);
                        request.send(response);

                        request.onloadend = function () {
                            window.location = "/Home/Products";
                        }
                    }
                });

            }
            else {
                // The validation failed.
                document.getElementById("content1").style.display = "none";
                document.getElementById("content2").style.display = "block";
            }
        });
    }
    else {
        // The validation failed.
        document.getElementById("content1").style.display = "none";
        document.getElementById("content2").style.display = "block";
    }
}

// This method sends a message to ABC Retail's storage system.
function SendMessage(message) {
    POST_DATA("https://abcretail-st10069070.azurewebsites.net/api/SendMessage", message, function (request) {
        // The request succeeded.
        document.getElementById("txtMessage").value = "";

        document.getElementById("content1").style.display = "none";
        document.getElementById("content2").style.display = "block";
    });
}

// Closes the confirmation message for sending a messsage.
function CloseMessage() {
    document.getElementById("content1").style.display = "block";
    document.getElementById("content2").style.display = "none";
}

// Highlights the Quick Navigation menu.
function QuickNav_MouseHover(e) {
    e.srcElement.style.backgroundColor = "#286995"
    e.srcElement.style.color = "white";
}

// Removes the highlights from the Quick Navigation menu.
function QuickNav_MouseLeave(e) {
    e.srcElement.style.backgroundColor = 'white';
    e.srcElement.style.color = "black";
}

// Performs the operation of buying a product.
function BuyProduct(productId, userName, mode) {
    let requestData = { "ProductId": productId, "UserName": userName, "Mode": mode };
    POST_DATA("https://abcretail-st10069070.azurewebsites.net/api/BuyProduct", JSON.stringify(requestData), function (request) {
        // Redirect the user to the BuyProduct page.
        window.location = "/Home/BuyProduct/?Action=" + mode + "&ProductId=" + productId;

    });
}

function GET_DATA(url, e) {
    let request = new XMLHttpRequest();
    request.open("GET", "/Home/GetData", true);
    request.setRequestHeader("URL", url);
    request.send();

    if (e != null) {
        request.onloadend = function () {
            e(request.status, request.response);
        }
    }
}

function POST_DATA(url, data, e) {
    let request = new XMLHttpRequest();
    request.open("POST", "/Home/PostData", true);
    request.setRequestHeader("URL", url);
    request.send(data);

    if (e != null) {
        request.onloadend = function () {
            e(request);
        }
    }
}
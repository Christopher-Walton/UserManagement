using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Routing;
using UserManagement.Identity.UI.CustomAuthorization;
using UserManagement.Identity.UI.Models.Helper;
using UserManagementAPI.Common.Models;

namespace UserManagement.Identity.UI.Controllers
{
    public class BaseMvcController : Controller
    {
        //private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        protected CustomUserPrincipal GetCurrentUser()
        {
            return this.User as CustomUserPrincipal;
        }

        protected readonly string UserManagementAPI_URI = ConfigurationManager.AppSettings["UserManagementAPI"];

        public ApiException CreateApiException(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            // Create an anonymous object to use as the template for deserialization:
            var anonymousErrorObject =
                new { message = "", ModelState = new Dictionary<string, string[]>() };

            // Deserialize:
            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            // Now wrap into an exception which best fullfills the needs of your application:
            var ex = new ApiException(response);

            // Sometimes, there may be Model Errors:
            if (deserializedErrorObject != null && deserializedErrorObject.ModelState != null)
            {
                var errors =
                    deserializedErrorObject.ModelState
                                            .Select(kvp => string.Join(". ", kvp.Value));
                for (int i = 0; i < errors.Count(); i++)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(i, errors.ElementAt(i));
                }
            }
            // Othertimes, there may not be Model Errors:
            else
            {
                var error =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                foreach (var kvp in error)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(kvp.Key, kvp.Value);
                }
            }
            return ex;
        }

        protected void SendEmail(EmailModel model)
        {
            var resourceURI = @"Utility/SendEmail";

            try
            {
                var result = this.SendPostRequest<EmailModel, string>(resourceURI, model);
            }
            catch (ApiException ex)
            {
                throw ex;
            }
        }

        protected TResponse SendPostRequest<TRequest, TResponse>(string apiMethodURI, TRequest requestBody)
        {
            var resourceURI = @apiMethodURI;

            var endpoint = UserManagementAPI_URI + resourceURI;

            using (var client = new HttpClient())
            {
                var user = this.User as CustomUserPrincipal;

                if (user != null && !string.IsNullOrEmpty(user.GetAuthorizationToken()))
                {
                    //if (!string.IsNullOrEmpty(authorizationHeader))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.GetAuthorizationToken());
                }

                // Pass in an anonymous object that maps to the expected
                // RegisterUserBindingModel defined as the method parameter
                // for the Register method on the API:
                HttpResponseMessage response = null;

                response = client.PostAsJsonAsync<TRequest>(endpoint, requestBody).Result;

                if (!response.IsSuccessStatusCode)
                {
                    // Unwrap the response and throw as an Api Exception:
                    var ex = CreateApiException(response);

                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    throw ex;
                }
                else
                {
                    var apiResponse = response.Content.ReadAsAsync<TResponse>();

                    return apiResponse.Result;
                }
            }
        }

        protected TResponse SendGetRequest<TResponse>(string apiMethodURI)
        {
            var resourceURI = @apiMethodURI;

            var endpoint = UserManagementAPI_URI + resourceURI;

            using (var client = new HttpClient())
            {
                var user = this.User as CustomUserPrincipal;

                if(user != null && !string.IsNullOrEmpty(user.GetAuthorizationToken()))
                {
                    //if (!string.IsNullOrEmpty(authorizationHeader))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.GetAuthorizationToken());
                }
                // Pass in an anonymous object that maps to the expected
                // RegisterUserBindingModel defined as the method parameter
                // for the Register method on the API:

                HttpResponseMessage response = null;

                response = client.GetAsync(endpoint).Result;

                if (!response.IsSuccessStatusCode)
                {
                    // Unwrap the response and throw as an Api Exception:
                    var ex = CreateApiException(response);

                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    throw ex;
                }
                else
                {
                    var apiResponse = response.Content.ReadAsAsync<TResponse>();

                    return apiResponse.Result;
                }
            }
        }

        protected string GenerateEmailConfirmationToken(string emailAddress)
        {
            var resourceURI = @"UserManagement/GenerateEmailConfirmationToken";

            try
            {
                var token = this.SendPostRequest<string, string>(resourceURI, emailAddress);

                return token;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
        }

        protected void PrepareAndSendConfirmationEmail(string emailAddress, string userId, string userName)
        {
            try
            {
                var token = GenerateEmailConfirmationToken(emailAddress);

                string confirmEmailAddressURI = "";

                if (token != null)
                {
                    //encode the url to ensure that special characters,
                    //don't affect the validity of the linl
                    confirmEmailAddressURI = Url.Action("ConfirmEmail", "SignUp",
                                                    routeValues: new RouteValueDictionary(new
                                                    {
                                                        code = Url.Encode(token),
                                                        userId = Url.Encode(userId)
                                                    }), /* specify if needed */
                                                    protocol: Request.Url.Scheme /* This is the trick */);

                    //create email body, with clickable link for confirming the email address
                    var callbackUrl2 = string.Format("Please click <a href='{0}'> here <a/> to confirm your email address for your new Brilliant Engineering Account. Your username is : {1}",
                    confirmEmailAddressURI, userName);

                    SendEmail(new EmailModel()
                    {
                        Body = callbackUrl2,
                        EmailAddress = emailAddress,
                        Subject = "Confirm Your Email Address"
                    });
                }
            }
            catch (ApiException ex)
            {
                throw new EmailException();
            }
        }
    }
}
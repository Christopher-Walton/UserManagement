using NLog;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using UserManagement.Identity.UI.Models.Helper;
using UserManagementAPI.Common.Models;

namespace UserManagement.Identity.UI.Controllers
{
    [RoutePrefix("app/SignUp")]
    [AllowAnonymous]
    public class SignUpController : BaseMvcController
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUpCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpCompany(CompanyRegistrationModel companyRegistration)
        {
            if (!ModelState.IsValid)
                return View("SignUpCompany", companyRegistration);

            var resourceURI = @"Registration/RegisterCompany";

            UserReturnModel createdUser = null;

            try
            {
                createdUser = this.SendPostRequest<CompanyRegistrationModel, UserReturnModel>(resourceURI, companyRegistration);

                PrepareAndSendConfirmationEmail(createdUser.Email, createdUser.Id, createdUser.UserName);

                TempData["IsEmailSentSuccessfully"] = true;

                return RedirectToAction("SignUpComplete", "SignUp", new RouteValueDictionary(createdUser));
            }
            catch (ApiException ex)
            {
                return View("SignUpCompany", companyRegistration);
            }
            catch (EmailException emailException)
            {
                TempData["IsEmailSentSuccessfully"] = false;

                return RedirectToAction("SignUpComplete", "SignUp", new RouteValueDictionary(createdUser));
            }
        }

        [HttpGet]
        public ActionResult SignUpIndividual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpIndividual(IndividualRegistration individualRegistration)
        {
            if (!ModelState.IsValid)
                return View("SignUpIndividual", individualRegistration);

            var resourceURI = @"Registration/RegisterIndividual";
            UserReturnModel createdUser = null;
            try
            {
                createdUser = this.SendPostRequest<IndividualRegistration, UserReturnModel>(resourceURI, individualRegistration);

                PrepareAndSendConfirmationEmail(createdUser.Email, createdUser.Id, createdUser.UserName);

                TempData["IsEmailSentSuccessfully"] = true;

                return RedirectToAction("SignUpComplete", "SignUp", new RouteValueDictionary(createdUser));
            }
            catch (ApiException ex)
            {
                return View("SignUpIndividual", individualRegistration);
            }
            catch (EmailException emailException)
            {
                TempData["IsEmailSentSuccessfully"] = false;

                return RedirectToAction("SignUpComplete", "SignUp", new RouteValueDictionary(createdUser));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUpPayAndGo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpPayAndGo(PayAndGoRegistration payNoGoRegistration)
        {
            
            _logger.Info("Entered Sign Up Page!");

            if (!ModelState.IsValid)
                return View("SignUpPayAndGo", payNoGoRegistration);

            var resourceURI = @"Registration/RegisterIndividual";
            UserReturnModel createdUser = null;


            _logger.Info(payNoGoRegistration.AgreeToPayandGoConditions);

            if (!payNoGoRegistration.AgreeToPayandGoConditions)
            {
                ModelState.AddModelError("AgreeToPayandGoConditions", "In order to use the pay and go service you must click the agreement checkbox");
                return View("SignUpPayAndGo", payNoGoRegistration);
            }

            try
            {
                createdUser = this.SendPostRequest<IndividualRegistration, UserReturnModel>(resourceURI, payNoGoRegistration);

                PrepareAndSendConfirmationEmail(createdUser.Email, createdUser.Id, createdUser.UserName);

                TempData["IsEmailSentSuccessfully"] = true;

                _logger.Debug("PayAndGoAttemptedHere");

                #region Pay and Go Specific Stuff

                var resourceURI2 = @"Registration/PayAndGoIndividual";

                var payAndGoResult = this.SendPostRequest<PayAndGoRegistration, string>(resourceURI2, payNoGoRegistration);

                #endregion Pay and Go Specific Stuff

                return RedirectToAction("SignUpComplete", "SignUp", new RouteValueDictionary(createdUser));
            }
            catch (ApiException ex)
            {
                return View("SignUpIndividual", payNoGoRegistration);
            }
            catch (EmailException emailException)
            {
                TempData["IsEmailSentSuccessfully"] = false;

                return RedirectToAction("SignUpComplete", "SignUp", new RouteValueDictionary(createdUser));
            }
        }

        [HttpGet]
        public ActionResult SignUpComplete(UserReturnModel userReturnModel)
        {
            ViewBag.IsEmailSent = TempData["IsEmailSentSuccessfully"];

            return View(userReturnModel);
        }

        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        [HttpGet]
        public ActionResult ConfirmEmail(string userId = "", string code = "")
        {
            var resourceURI = @"UserManagement/ConfirmEmail";

            var endpoint = UserManagementAPI_URI + resourceURI;

            using (var client = new HttpClient())
            {
                var response =
                    client.GetAsync(endpoint + string.Format("?userId={0}&code={1}", userId, code)).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var ex = CreateApiException(response);

                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    //TODO
                    //REFACTOR THIS METHOD TO USE THE SEND GET REQUEST OF THE UNDERLYING API CONTROLLER
                    return null;//View(individualRegistration); TODO
                    //
                }
                else
                {
                    return RedirectToAction("EmailConfirmed", "SignUp");
                }
            }
        }

        [HttpGet]
        public ActionResult EmailConfirmed()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DownloadPdf()
        {
            return File("~/Download/PayAndGoTermsConditions.pdf", "application/pdf", Server.UrlEncode("PayAndGoTermsConditions.pdf"));
        }


        private string GenerateEmailConfirmationToken(string emailAddress)
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

        private void PrepareAndSendConfirmationEmail(string emailAddress, string userId, string userName)
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
using System.Web.Mvc;
using System.Web.Routing;
using UserManagement.Identity.UI.Models.Helper;
using UserManagementAPI.Common.Models;

namespace UserManagement.Identity.UI.Controllers
{
    [RoutePrefix("app/Reset")]
    public class ResetController : BaseMvcController
    {
        // GET: Reset
        public ActionResult RequestResetPasswordEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RequestResetPasswordEmail(string EmailAddress)
        {
            if (string.IsNullOrEmpty(EmailAddress))
            {
                ModelState.AddModelError("Email", "Email Address must be entered");
                return View();
            }

            //request token here
            var resourceURI = @"Authentication/RequestResetPasswordToken";

            var endpoint = UserManagementAPI_URI + resourceURI;

            try
            {
                var passwordResetToken = this.SendPostRequest<string, string>(resourceURI, EmailAddress);

                var resetPasswordLink = Url.Action("ResetPassword", "Reset",
                                            routeValues: new RouteValueDictionary(new { rtoken = passwordResetToken, emailAddress = EmailAddress }), /* specify if needed */
                                            protocol: Request.Url.Scheme /* This is the trick */);

                var callbackUrl2 = string.Format("Please click <a href='{0}'> here <a/> to reset your password for your Brilliant Engineering account.", resetPasswordLink);

                SendEmail(new EmailModel()
                {
                    Body = callbackUrl2,
                    EmailAddress = EmailAddress,
                    Subject = "Reset Password Link"
                });

                return RedirectToAction("ResetEmailSent", "Reset", new RouteValueDictionary(new { emailAddress = EmailAddress }));
            }
            catch (ApiException ex)
            {
                //TODO
                //Error here
                return View("RequestResetPasswordEmail");
            }
        }

        public ActionResult ResetEmailSent(string emailAddress)
        {
            ViewBag.Email = emailAddress;
            return View();
        }

        //TODO
        //validate arguements
        [Route("ResetPassword", Name = "ResetPassword")]
        [HttpGet]
        public ActionResult ResetPassword(string rtoken, string emailAddress)
        {
            var model = new ResetPasswordModel();
            model.PasswordResetToken = rtoken;
            model.EmailAddress = emailAddress;

            //ViewBag.ResetToken = rtoken;
            //ViewBag.EmailAddress = emailAddress;

            return View(model);
        }

        [HttpPost]
        [Route("ResetPassword", Name = "ResetPasswordPost")]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var resourceURI = @"Authentication/ResetPassword";

            try
            {
                var result = this.SendPostRequest<ResetPasswordModel, string>(resourceURI, model);

                return RedirectToAction("PasswordResetCompleted", "Reset", new RouteValueDictionary(new { emailAddress = model.EmailAddress }));
            }
            catch (ApiException ex)
            {
                //TODO
                return View(model);
            }
        }

        public ActionResult PasswordResetCompleted(string emailAddress)
        {
            ViewBag.Email = emailAddress;
            return View();
        }
    }
}
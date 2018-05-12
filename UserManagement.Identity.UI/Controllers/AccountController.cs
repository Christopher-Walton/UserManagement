using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserManagement.Identity.UI.CustomAuthorization;
using UserManagement.Identity.UI.Models.Helper;
using UserManagementAPI.Common.Models;

namespace UserManagement.Identity.UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseMvcController
    {
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(AuthenticateUserModel userModel)
        {
            if (!ModelState.IsValid)
                return View(userModel);

            var resourceURI = @"Authentication/AuthenticateUser";

            try
            {
                var userData = this.SendPostRequest<AuthenticateUserModel, UserDataModel>(resourceURI, userModel);

                var dataForAuthenticationDetails = new UserData();

                dataForAuthenticationDetails.Roles = userData.Roles;
                dataForAuthenticationDetails.WebServiceToken = userData.AuthorizationToken;
                dataForAuthenticationDetails.UserId = userData.UserId;
                dataForAuthenticationDetails.UserName = userData.UserName;

                var authTicket = new FormsAuthenticationTicket(2,
                    userModel.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(5),
                    false,
                    dataForAuthenticationDetails.Serialize()
                );

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                {
                    HttpOnly = true
                };

                Response.AppendCookie(authCookie);

                return RedirectToAction("MyProfile", "Profile");
            }
            catch (ApiException ex)
            {
                return View(userModel);
            }
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("SignIn", "Account");
        }

        public ActionResult LoggedInStatus()
        {
            if (CheckLoggedInUser())
            {
                ViewBag.IsLoggedIn = true;
                var userName = System.Web.HttpContext.Current.User.Identity.Name;
                ViewBag.SignOutMessage = String.Format("Sign Out {0}", System.Web.HttpContext.Current.User.Identity.Name);
            }
            else
            {
                ViewBag.IsLoggedIn = false;
            }

            return PartialView();
        }

        private bool CheckLoggedInUser()
        {
            var isUserLoggedIn = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            return isUserLoggedIn;
        }

        [ChildActionOnly]
        public ActionResult AdministratorMenu()
        {
            if (CheckLoggedInUser() && User.IsInRole("Administrator"))
            {
                //ViewBag.IsLoggedIn = true;
                return PartialView();
                //var userName = System.Web.HttpContext.Current.User.Identity.Name;
                //ViewBag.SignOutMessage = String.Format("Sign Out {0}", System.Web.HttpContext.Current.User.Identity.Name);
            }
            else
            {
                return null;
                //ViewBag.IsLoggedIn = false;
            }
        }
    }
}
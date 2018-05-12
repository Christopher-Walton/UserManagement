using System.Web.Mvc;
using UserManagement.Identity.UI.CustomAuthorization;
using UserManagement.Identity.UI.Models.Helper;
using UserManagementAPI.Common.Models;

namespace UserManagement.Identity.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class ProfileController : BaseMvcController
    {
        [HttpGet]
        public ActionResult MyProfile()
        {
            var user = this.User as CustomUserPrincipal;

            ViewBag.Message = TempData["Message"] as string;           

            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordBindingModel model)
        {
            string resourceUri = @"UserManagement/ChangePassword";

            try
            {
                var result = this.SendPostRequest<ChangePasswordBindingModel, string>(resourceUri, model);

                TempData.Add("Message", "Your password has been sucessfully updated.");

                return RedirectToAction("MyProfile");
            }
            catch (ApiException ex)
            {
                //TODO
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var apiEndpoint = @"UserManagement/GetUserDetails";

            var currentUser = this.User as CustomUserPrincipal;
            try
            {
                var editUserModel = this.SendGetRequest<EditUserModel>(apiEndpoint);
                return View(editUserModel);
            }
            catch (ApiException ex)
            {
                //TODO Where does this error go!!!
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditProfile(EditUserModel userModel)
        {
            var apiEndpoint = @"UserManagement/UpdateUserDetails";

            try
            {
                var editUserModel = this.SendPostRequest<EditUserModel, string>(apiEndpoint, userModel);

                TempData.Add("Message", "Profile Updated Successfully");

                return RedirectToAction("MyProfile");
            }
            catch (ApiException ex)
            {
                //TODO Where does this error go!!!
                return View();
            }
        }
    }
}
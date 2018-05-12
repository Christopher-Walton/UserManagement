using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UserManagement.Identity.UI.CustomAuthorization;
using UserManagement.Identity.UI.Models.Helper;
using UserManagementAPI.Common.Models;

namespace UserManagement.Identity.UI.Controllers
{
    [AuthorizeUser(Roles = "Administrator")]
    public class AdministratorController : BaseMvcController
    {
        public ActionResult Index()
        {
            var resourceURI = @"Administration/users";

            try
            {
                var token = this.GetCurrentUser().GetAuthorizationToken();

                var usersList = this.SendGetRequest<IEnumerable<UserReturnModel>>(resourceURI);

                return View(usersList);
            }
            catch (ApiException ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult ViewUser(string userName)
        {
            //TODO ADD PARAMETER VALIDATION

            var resourceURI = string.Format(@"Administration/user/{0}", userName);

            try
            {
                var token = this.GetCurrentUser().GetAuthorizationToken();

                var usersList = this.SendGetRequest<UserReturnModel>(resourceURI);

                ViewBag.Message = TempData["Message"];

                return View(usersList);
            }
            catch (ApiException ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditUserRoles(string userId)
        {
            try
            {
                var resourceURI = @"roles";

                var roles = SendGetRequest<IEnumerable<RoleReturnModel>>(resourceURI);

                //TODO REFACTOR
                //JUST REMOVE THE DEFAULT SYSTEM USER AS IT DOESNT NEED TO BE SHOWN
                roles = roles.Where(c => c.Name != "User");

                var resourceURI2 = string.Format(@"Administration/user/{0}", userId);

                var userData = SendGetRequest<UserReturnModel>(resourceURI2);

                ViewBag.PersonName = userData.FullName;

                var rolesViewModel = new RolesViewModel()
                {
                    AvailableRoles = roles.Select(c => new Role { Id = c.Id, Name = c.Name, }).ToList(),
                    SelectedRoles = roles.Select(c => new Role { Id = c.Id, Name = c.Name }).Where(d => userData.Roles.Contains(d.Name)).ToList()
                };

                return View(rolesViewModel);
            }
            catch (ApiException ex)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditUserRoles(RolesViewModel userRoles)
        {
            try
            {
                var resourceURI = string.Format("roles/user/{0}/roles", userRoles.UserId);

                IEnumerable<string> roles = userRoles != null && userRoles.PostedRoles != null ?
                    userRoles.PostedRoles.RoleIds.ToList() : new List<string>();

                this.SendPostRequest<IEnumerable<string>, string>(resourceURI, roles);

                TempData.Add("Message", "User roles updated successfully.");

                return RedirectToAction("Index");
            }
            catch (ApiException ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult SetUserPassword(string userId)
        {
            try
            {
                var resourceURI2 = string.Format(@"Administration/user/{0}", userId);

                var userData = SendGetRequest<UserReturnModel>(resourceURI2);

                return View(new SetPasswordModel
                {
                    UserId = userData.Id,
                    UserName = userData.UserName,
                    Name = userData.FullName
                });
            }
            catch (ApiException ex)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult SetUserPassword(SetPasswordModel model)
        {
            //TODO FORMAL VALIDATION HERE

            try
            {
                var resourceURI = "Administration/user/SetUserPassword";

                var userData = this.SendPostRequest<SetPasswordModel, string>(resourceURI, model);

                TempData.Add("Message", string.Format("Password for user {0},has been set sucessfully", model.UserName));

                return RedirectToAction("ViewUser", new { userName = model.UserName });
            }
            catch (ApiException ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult ResendConfirmationEmail(string userName, string userId, string emailAddress)
        {
            //TODO
            //VALIDATE PARAMETERS HERE

            PrepareAndSendConfirmationEmail(emailAddress, userId, userName);

            TempData.Add("Message", string.Format("Confirmation Email Sent Successfully!"));

            return RedirectToAction("ViewUser", new { userName = userName });
        }
    }
}
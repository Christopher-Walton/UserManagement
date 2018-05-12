using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;
using UserManagementAPI.Identity.WebAPI.CustomAuthorization;

namespace UserManagementAPI.Identity.WebAPI.Controllers
{
    [RoutePrefix("api/Administration")]
    public class AdministrationController : BaseApiController
    {
        private IUserManagement _userManagement;

        public AdministrationController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }

        [Route("users")]
        [HttpGet]
        [AuthorizationAction(Roles = "Administrator")]
        public HttpResponseMessage Users()
        {
            HttpResponseMessage response = null;

            var users = _userManagement.GetUsers();

            response = Request.CreateResponse<IEnumerable<UserReturnModel>>(HttpStatusCode.OK, users);

            return response;
        }

        [Route("user/{id:guid}", Name = "GetUserById")]
        [HttpGet]
        [AuthorizationAction(Roles = "Administrator")]
        public IHttpActionResult GetUserById(string Id)
        {
            var user = _userManagement.FindByUserId(Id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [Route("user/{username}", Name = "GetUserByName")]
        [HttpGet]
        [AuthorizationAction(Roles = "Administrator")]
        public IHttpActionResult GetUserByName(string username)
        {
            var user = _userManagement.FindByUserName(username);

            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [Route("user/SetUserPassword", Name = "SetUserPassword")]
        [HttpPost]
        [AuthorizationAction(Roles = "Administrator")]
        public IHttpActionResult SetUserPassword(SetPasswordModel passwordModel)
        {
            var user = _userManagement.FindByUserId(passwordModel.UserId);

            if (user != null)
            {
                var result = _userManagement.SetUserPassword(passwordModel);

                if (result.IsSuccess)
                    return Ok(result.Result);
                else
                    return GetErrorResult(result);
            }

            return NotFound();
        }

        //[Route("user/delete/{username}", Name = "DeleteUser")]
        //[HttpDelete]
        //[AuthorizationAction(Roles = "Administrator")]
        //public IHttpActionResult DeleteUser(string username)
        //{
        //    var result = _userManagement.DeleteUser(username);

        //    if (result.IsSuccess)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        if (result.Errors != null && result.Errors.Count() > 0)
        //            return GetErrorResult(result);
        //        else
        //            return InternalServerError();
        //    }
        //}
    }
}
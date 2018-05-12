using System;
using System.Web.Http;
using UserManagementAPI.BusinessLogic;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;
using UserManagementAPI.Identity.WebApi.CustomValidation;
using UserManagementAPI.Identity.WebAPI.CustomAuthorization;

namespace UserManagementAPI.Identity.WebAPI.Controllers
{
    [RoutePrefix("api/UserManagement")]
    public class UserManagementController : BaseApiController
    {
        private readonly IUserManagement _userManagement;
        private readonly IUtilityManager _utilityManager;

        public UserManagementController(IUserManagement userManagement, IUtilityManager utilityManager)
        {
            _userManagement = userManagement;
            _utilityManager = utilityManager;
        }

        [Route("Create")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public IHttpActionResult CreateUser(CreateUserBindingModel createUserModel)
        {
            var createUserActionResult = _userManagement.CreateUser(createUserModel);

            if (!createUserActionResult.IsSuccess)
            {
                return GetErrorResult(createUserActionResult);
            }

            var userId = createUserActionResult.Result.Id;

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = userId }));

            return Created(locationHeader, createUserActionResult.Result);
        }

        [Route("GenerateEmailConfirmationToken", Name = "GenerateEmailConfirmationToken")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GenerateEmailConfirmationToken([FromBody]string emailAddress)
        {
            var userReturnModel = _userManagement.FindByEmailAddress(emailAddress);

            if (userReturnModel != null)
            {
                var createConfirmationTokenResult = _userManagement.GenerateEmailConfirmationToken(userReturnModel.Id);

                if (createConfirmationTokenResult.IsSuccess)
                    return Ok(createConfirmationTokenResult.Result);
                else

                    return GetErrorResult(createConfirmationTokenResult);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            var result = _userManagement.ConfirmEmail(userId, code);

            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        [AuthorizationAction]
        [ValidateModel]
        public IHttpActionResult ChangePassword(ChangePasswordBindingModel model)
        {
            CustomUserPrincipal currentUser = User as CustomUserPrincipal;

            var changeUserPasswordResult = _userManagement.ChangeUserPassword(currentUser.GetUserId(), model);

            if (!changeUserPasswordResult.IsSuccess)
            {
                return GetErrorResult(changeUserPasswordResult);
            }

            return Ok("Password Updated Successfully!");
        }

        [Route("GetUserDetails")]
        [HttpGet]
        [AuthorizationAction]
        public IHttpActionResult GetUserDetails()
        {
            var user = this.User as CustomUserPrincipal;

            var userReturnModel = _userManagement.FindByUserId(user.GetUserId());

            if (userReturnModel == null)
                return NotFound();
            else
            {
                var userModel = new EditUserModel()
                {
                    FirstName = userReturnModel.FirstName,
                    LastName = userReturnModel.LastName,
                    MobileNumber = userReturnModel.MobileNumber,
                    UserId = userReturnModel.Id
                };

                return Ok<EditUserModel>(userModel);
            }
        }

        [Route("UpdateUserDetails")]
        [HttpPost]
        [AuthorizationAction]
        public IHttpActionResult UpdateUserDetails(EditUserModel editUserModel)
        {
            var updateUserResult = _userManagement.UpdateUser(editUserModel);

            if (updateUserResult.IsSuccess)
                return Ok("User Profile Updated Successfully");
            else
            {
                return GetErrorResult(updateUserResult);
            }
        }
    }
}
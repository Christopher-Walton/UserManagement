using System.Linq;
using System.Web.Http;
using UserManagementAPI.BusinessLogic;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;
using UserManagementAPI.Identity.WebApi.CustomValidation;

namespace UserManagementAPI.Identity.WebAPI.Controllers
{
    [RoutePrefix("api/Authentication")]
    public class AuthenticationController : BaseApiController
    {
        private IAuthenticationManagement _authenticationManagement;
        private IAuthorizationManager _authorizationManager;

        public AuthenticationController(IAuthenticationManagement authenticationManagement, IAuthorizationManager authorizationManager)
        {
            _authenticationManagement = authenticationManagement;
            _authorizationManager = authorizationManager;
        }

        [Route("CreateToken")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public IHttpActionResult CreateToken([FromBody] AuthenticateUserModel authenticateUserModel)
        {
            var result = _authorizationManager.CreateAuthorizationToken(authenticateUserModel.UserName, authenticateUserModel.Password);

            if (result.Errors != null && result.Errors.Any())
            {
                return GetErrorResult(result);
            }

            return Ok(result.Result);
        }

        [Route("AuthenticateUser")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public IHttpActionResult AuthenticateUser([FromBody] AuthenticateUserModel authenticateUserModel)
        {
            var authenticationResult = _authorizationManager.AuthenticateUser(authenticateUserModel.UserName, authenticateUserModel.Password);

            if (authenticationResult.Errors != null && authenticationResult.Errors.Any())
            {
                return GetErrorResult(authenticationResult);
            }

            return Ok(authenticationResult.Result);
        }

        //TODO
        //POSSIBLE SECURITY RISK
        //API KEY WILL BE NECESSARY
        [Route("RequestResetPasswordToken")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult RequestResetPasswordToken([FromBody] string emailAddress)
        {
            var result = _authenticationManagement.GeneratePasswordResetToken(emailAddress);

            if (result.IsSuccess)
            {
                return Ok(result.Result);
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        //[Route("RequestResetPasswordEmail")]
        //[HttpPost]
        //[AllowAnonymous]
        //public IHttpActionResult RequestResetPasswordEmail([FromBody] string emailAddress)
        //{
        //    var result = _authenticationManagement.GeneratePasswordResetToken(emailAddress);

        //    if (result.IsSuccess)
        //    {
        //        _authenticationManagement.SendEmailViaEmail(emailAddress, "Password Reset Token", string.Format("<h2> Your Password Reset Token: {0} </h2>", result.Result));
        //        return Ok("Password Reset Email Sent Sucessfully!");
        //    }
        //    else
        //    {
        //        return GetErrorResult(result);
        //    }
        //}
        
        [Route("ResetPassword")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public IHttpActionResult ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            var result = _authenticationManagement.ResetUserPassword(resetPasswordModel.EmailAddress, resetPasswordModel.PasswordResetToken, resetPasswordModel.Password);

            if (result.IsSuccess)
            {
                return Ok("Password reset sucessfully!");
            }
            else
            {
                return GetErrorResult(result);
            }
        }
    }
}
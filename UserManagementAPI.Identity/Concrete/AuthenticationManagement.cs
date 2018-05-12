using System.Collections.Generic;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;

namespace UserManagementAPI.Identity.Concrete
{
    public class AuthenticationManagement : IAuthenticationManagement
    {
        private readonly IUserManager _userManager;
        private readonly IModelFactory _modelFactory;

        public AuthenticationManagement(IUserManager userManager, IModelFactory modelFactory)
        {
            _modelFactory = modelFactory;
            _userManager = userManager;
        }

        public GenericActionResult<UserDataModel> AuthenticateUser(string userName, string password)
        {
            return _userManager.AuthenticateUser(userName, password);
        }

        public GenericActionResult<string> GeneratePasswordResetToken(string emailAddress)
        {
            var user = _userManager.GetUserByEmailAddress(emailAddress);

            if (user != null)
            {
                return _userManager.GenerateTokenForPasswordReset(user.UserName);
            }
            else
            {
                return new GenericActionResult<string>() { IsSuccess = false, Errors = new List<string>() { "No user exists with this email address" } };
            }
        }

        public GenericActionResult<string> RequestResetPasswordToken(string userName)
        {
            var user = _userManager.GenerateTokenForPasswordReset(userName);

            return user;
        }

        public GenericActionResult<string> ResetUserPassword(string emailAddress, string passwordResetToken, string newPassword)
        {
            return _userManager.ResetUserPassword(emailAddress, passwordResetToken, newPassword);
        }

        public void SendEmail(string userName, string subject, string body)
        {
            _userManager.SendEmailToUser(userName, subject, body);
        }

        public void SendEmailViaEmail(string email, string subject, string body)
        {
            var user = _userManager.GetUserByEmailAddress(email);

            _userManager.SendEmailToUser(user.UserName, subject, body);
        }
    }
}
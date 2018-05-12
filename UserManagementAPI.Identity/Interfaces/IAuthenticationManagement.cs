using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;

namespace UserManagementAPI.Identity.UserManagementInterfaces
{
    public interface IAuthenticationManagement
    {
        GenericActionResult<UserDataModel> AuthenticateUser(string userName, string password);

        GenericActionResult<string> RequestResetPasswordToken(string userName);

        GenericActionResult<string> GeneratePasswordResetToken(string emailAddress);

        //TODO
        //consider placing in a utility controller/module
        void SendEmail(string userName, string subject, string body);

        void SendEmailViaEmail(string email, string subject, string body);

        GenericActionResult<string> ResetUserPassword(string userName, string passwordResetToken, string newPassword);
    }
}
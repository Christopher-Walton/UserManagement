using System.Collections.Generic;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;

namespace UserManagementAPI.Identity
{
    public interface IUserManager
    {
        GenericActionResult<string> AddUserToRoles(string userId, IEnumerable<string> roles);

        GenericActionResult<UserDataModel> AuthenticateUser(string userName, string password);

        GenericActionResult<string> ChangeUserPassword(string userId, string oldPassword, string newPassword);

        GenericActionResult<string> ConfirmUserEmail(string userId, string code);

        GenericActionResult<UserReturnModel> CreateUser(CreateUserBindingModel user, string password);

        GenericActionResult<string> DeleteUserByUserName(string userName);

        UserReturnModel FindUser(string userName, string password);

        GenericActionResult<string> GenerateEmailConfirmationTokenForUser(string userId);

        GenericActionResult<string> GenerateTokenForPasswordReset(string userName);

        UserReturnModel GetUserByEmailAddress(string emailAddress);

        UserReturnModel GetUserById(string userId);

        UserReturnModel GetUserByName(string userName);

        IList<string> GetUserRoles(string userId);

        IEnumerable<UserReturnModel> GetUsers();

        GenericActionResult<string> RemoveUserFromRole(string userId, string role);

        GenericActionResult<string> RemoveUserFromRoles(string userId, IEnumerable<string> roles);

        GenericActionResult<string> ResetUserPassword(string emailAddress, string passwordResetToken, string newPassword);

        GenericActionResult<string> SendEmailToUser(string userName, string subject, string body);

        GenericActionResult<string> SendEmailToUserViaEmail(string emailAddress, string subject, string body);

        GenericActionResult<string> SetPassword(SetPasswordModel setPasswordModel);

        GenericActionResult<string> UpdateUser(EditUserModel editUserModel);
    }
}
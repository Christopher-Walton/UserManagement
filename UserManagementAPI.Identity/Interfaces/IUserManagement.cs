using System.Collections.Generic;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;

namespace UserManagementAPI.Identity.UserManagementInterfaces
{
    public interface IUserManagement
    {
        GenericActionResult<string> ChangeUserPassword(string userId, ChangePasswordBindingModel model);

        GenericActionResult<string> ConfirmEmail(string userId, string code);

        GenericActionResult<UserReturnModel> CreateUser(CreateUserBindingModel userToCreate);

        GenericActionResult<string> DeleteUser(string userName);

        UserReturnModel FindByEmailAddress(string emailAddress);

        UserReturnModel FindByUserId(string userId);

        UserReturnModel FindByUserName(string userName);

        GenericActionResult<string> GenerateEmailConfirmationToken(string userId);

        UserReturnModel GetUser(string userId);

        IEnumerable<UserReturnModel> GetUsers();

        GenericActionResult<string> SetUserPassword(SetPasswordModel model);

        GenericActionResult<string> UpdateUser(EditUserModel editUserModel);
    }
}
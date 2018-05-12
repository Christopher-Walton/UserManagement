using System;
using System.Collections.Generic;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;

namespace UserManagementAPI.Identity.Concrete
{
    public class UserManagement : IUserManagement
    {
        private readonly IUserManager _userManager;
        private ModelFactory _modelFactory;

        public UserManagement(ModelFactory modelFactory, IUserManager UserManager)
        {
            _modelFactory = modelFactory;
            _userManager = UserManager;
        }

        public GenericActionResult<string> ChangeUserPassword(string userId, ChangePasswordBindingModel model)
        {
            return _userManager.ChangeUserPassword(userId, model.OldPassword, model.ConfirmPassword);
        }

        public GenericActionResult<String> ConfirmEmail(string userId, string code)
        {
            var identityResult = _userManager.ConfirmUserEmail(userId, code);

            return identityResult;
        }

        public GenericActionResult<UserReturnModel> CreateUser(CreateUserBindingModel userToCreate)
        {
            return _userManager.CreateUser(userToCreate, userToCreate.Password);
        }

        public GenericActionResult<string> DeleteUser(string userName)
        {
            return _userManager.DeleteUserByUserName(userName);
        }

        public UserReturnModel FindByEmailAddress(string emailAddress)
        {
            return _userManager.GetUserByEmailAddress(emailAddress);
        }

        public UserReturnModel FindByUserId(string userId)
        {
            return _userManager.GetUserById(userId);
        }

        public UserReturnModel FindByUserName(string userName)
        {
            return _userManager.GetUserByName(userName);
        }

        public GenericActionResult<string> GenerateEmailConfirmationToken(string userId)
        {
            var code = _userManager.GenerateEmailConfirmationTokenForUser(userId);
            return code;
        }

        public UserReturnModel GetUser(string Id)
        {
            return _userManager.GetUserById(Id);
        }

        public IEnumerable<UserReturnModel> GetUsers()
        {
            return _userManager.GetUsers();
        }

        public GenericActionResult<string> SetUserPassword(SetPasswordModel model)
        {
            return _userManager.SetPassword(model);
        }

        public GenericActionResult<string> UpdateUser(EditUserModel editUserModel)
        {
            return _userManager.UpdateUser(editUserModel);
        }
    }
}
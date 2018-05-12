using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Infrastructure;
using UserManagementAPI.Identity.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;

namespace UserManagementAPI.Identity.Concrete
{
    public class UserManager : ApplicationUserManager, IUserManager
    {
        private readonly IModelFactory _modelFactory;
        private readonly IUserRoleManager _roleManager;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public UserManager(IModelFactory modelFactory, IUserStore<ApplicationUser> store, IUserRoleManager roleManager)
            : base(store)
        {
            _modelFactory = modelFactory;
            _roleManager = roleManager;
        }

        public GenericActionResult<string> AddUserToRoles(string userId, IEnumerable<string> roles)
        {
            var result = this.AddToRoles(userId, roles.ToArray());

            return new GenericActionResult<string> { Result = null, IsSuccess = result.Succeeded, Errors = result.Errors };
        }

        public GenericActionResult<UserDataModel> AuthenticateUser(string userName, string password)
        {
            var identityResult = this.Find(userName, password);

            UserDataModel userDataModel = null;

            if (identityResult != null)
            {
                if (identityResult.EmailConfirmed)
                {
                    userDataModel = _modelFactory.CreateUserDataModel(identityResult);

                    userDataModel.Roles =
                        _roleManager.GetRoleNames(identityResult.Roles.Select(c => c.RoleId).ToList()).ToList();

                    userDataModel.IsAuthenticated = true;

                    var authenticationResult = new GenericActionResult<UserDataModel>
                    {
                        Result = userDataModel,
                        IsSuccess = true
                    };
                    return authenticationResult;
                }
                else
                {
                    userDataModel = new UserDataModel { IsAuthenticated = false };
                    return new GenericActionResult<UserDataModel>
                    {
                        Result = userDataModel,
                        IsSuccess = false,
                        Errors = new List<string> { "Email has not been confirmed!" }
                    };
                }
            }
            else
            {
                userDataModel = new UserDataModel { IsAuthenticated = false };
                return new GenericActionResult<UserDataModel>
                {
                    Result = userDataModel,
                    IsSuccess = false,
                    Errors = new List<string> { "Incorrect username or password!" }
                };
            }
        }

        public GenericActionResult<string> ChangeUserPassword(string userId, string oldPassword, string newPassword)
        {
            var result = this.ChangePassword(userId, oldPassword, newPassword);

            var actionResult = new GenericActionResult<string>()
            {
                IsSuccess = result.Succeeded,
                Errors = result.Errors
            };

            return actionResult;
        }

        public GenericActionResult<string> ConfirmUserEmail(string userId, string code)
        {
            var result = this.ConfirmEmail(userId, code);

            if (result.Succeeded)
                return new GenericActionResult<string> { IsSuccess = true, Errors = null, Result = "User email confirmed" };
            else
                return new GenericActionResult<string> { IsSuccess = false, Errors = result.Errors };
        }

        public GenericActionResult<UserReturnModel> CreateUser(CreateUserBindingModel user, string password)
        {
            var newUser = new ApplicationUser()
            {
                UserName = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.MobileNumber,
            };

            try
            {
                IdentityResult addUserResult = this.Create(newUser, password);

                var result = new GenericActionResult<UserReturnModel>()
                {
                    IsSuccess = addUserResult.Succeeded,
                    Errors = addUserResult.Errors,
                    Result = addUserResult.Succeeded ? _modelFactory.Create(newUser) : null
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Creating user failed!");

                return new GenericActionResult<UserReturnModel>()
                {
                    Errors = new List<string>() { "An error occured while creating the user,please try again later." },
                    IsSuccess = false,
                    Result = null
                };
            }
        }

        public GenericActionResult<string> DeleteUserByUserName(string userName)
        {
            var user = this.FindByName(userName);

            if (user != null)
            {
                var result = this.Delete(user);

                if (result.Succeeded)
                    return new GenericActionResult<string>(isSuccess: true, errors: null,
                                                           Result: String.Format("User '{0}' deleted sucessfully", userName));
                else
                    return new GenericActionResult<string>(isSuccess: false, errors: result.Errors,
                                                           Result: null);
            }
            else
            {
                return new GenericActionResult<string>(isSuccess: true,
                    errors: new List<string> { string.Format("User '{0}' not found!", userName) }, Result: null);
            }
        }

        public UserReturnModel FindUser(string userName, string password)
        {
            var user = this.Find(userName, password);

            if (user != null)
            {
                var userReturnModel = _modelFactory.Create(user);
                userReturnModel.Roles = GetUserRoles(user.Id);
                return userReturnModel;
            }
            else
                return null;
        }

        public GenericActionResult<string> GenerateEmailConfirmationTokenForUser(string userId)
        {
            try
            {
                var token = this.GenerateEmailConfirmationToken(userId);
                return new GenericActionResult<string> { Result = token, IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error generating email confirmation token");
                return new GenericActionResult<string>
                {
                    Result = null,
                    IsSuccess = false,
                    Errors = new List<string>() { "Error generating email confirmation token" }
                };
            }
        }

        public GenericActionResult<string> GenerateTokenForPasswordReset(string userName)
        {
            var user = this.FindByName(userName);

            if (user == null)
            {
                return new GenericActionResult<string> { Errors = new List<string> { "No user with this user name exists" }, IsSuccess = false };
            }

            if (!user.EmailConfirmed)
            {
                return new GenericActionResult<string> { IsSuccess = false, Errors = new List<string> { "User email has not been confirmed." } };
            }
            else
            {
                string token = "";
                try
                {
                    token = this.GeneratePasswordResetToken(user.Id);

                    return new GenericActionResult<string>
                    {
                        IsSuccess = true,
                        Result = token
                    };
                }
                catch (Exception ex)
                {
                    _logger.Error("Error Generating token for user", ex);
                    return new GenericActionResult<string>
                    {
                        Errors = new List<string> { "error generating password reset token" },
                        IsSuccess = false,
                        Result = null
                    };
                }
            }
        }

        public UserReturnModel GetUserByEmailAddress(string emailAddress)
        {
            var user = this.FindByEmail(emailAddress);

            if (user != null)
                return _modelFactory.Create(user);
            else
                return null;
        }

        public UserReturnModel GetUserById(string userId)
        {
            var user = this.FindById(userId);

            if (user != null)
            {
                var model = _modelFactory.Create(user);

                var roles = this.GetUserRoles(user.Id);
                model.Roles = roles;
                return model;
            }
            else
                return null;
        }

        public UserReturnModel GetUserByName(string userName)
        {
            var user = this.FindByName(userName);

            if (user != null)
            {
                var model = _modelFactory.Create(user);
                var roles = this.GetUserRoles(user.Id);
                model.Roles = roles;
                return model;
            }
            else
                return null;
        }

        public IList<string> GetUserRoles(string userId)
        {
            var identityRoles = this.GetRoles(userId);

            return identityRoles;
        }

        public IEnumerable<UserReturnModel> GetUsers()
        {
            var users = this.Users.ToList();

            var usersModels = users.Select(c => _modelFactory.Create(c));

            return usersModels;
        }

        public GenericActionResult<string> RemoveUserFromRole(string userId, string role)
        {
            var result = this.RemoveFromRole(userId, role);

            return new GenericActionResult<string> { Result = null, IsSuccess = result.Succeeded, Errors = result.Errors };
        }

        public GenericActionResult<string> RemoveUserFromRoles(string userId, IEnumerable<string> roles)
        {
            var result = this.RemoveFromRoles(userId, roles.ToArray());

            return new GenericActionResult<string> { Result = null, IsSuccess = result.Succeeded, Errors = result.Errors };
        }

        public GenericActionResult<string> ResetUserPassword(string emailAddress, string passwordResetToken, string newPassword)
        {
            var user = this.FindByEmail(emailAddress);

            if (user == null)
            {
                return new GenericActionResult<string>() { IsSuccess = false, Errors = new List<string>() { "No user exists with this email address!" }, };
            }

            var result = this.ResetPassword(user.Id, passwordResetToken, newPassword);

            if (result.Succeeded)
                return new GenericActionResult<string> { IsSuccess = true };
            else
                return new GenericActionResult<string> { IsSuccess = false, Errors = result.Errors };
        }

        public GenericActionResult<string> SendEmailToUser(string userName, string subject, string body)
        {
            var user = this.FindByName(userName);

            if (user != null)
            {
                try
                {
                    this.SendEmail(user.Id, subject, body);
                    return new GenericActionResult<string>() { IsSuccess = true, Result = string.Format("Email sent successfully to {0}", userName) };
                }
                catch (Exception ex)
                {
                    _logger.Error("Error Sending Email", ex);

                    return new GenericActionResult<string>() { IsSuccess = false, Errors = new List<string>() { "Error occured during sending email to user" } };
                }
            }
            else
            {
                return new GenericActionResult<string>() { IsSuccess = true, Result = string.Format("No user exists with user name: {0}", userName) };
            }
        }

        public GenericActionResult<string> SendEmailToUserViaEmail(string emailAddress, string subject, string body)
        {
            var user = this.GetUserByEmailAddress(emailAddress);

            if (user != null)
            {
                try
                {
                    this.SendEmail(user.Id, subject, body);
                    return new GenericActionResult<string>() { IsSuccess = true, Result = string.Format("Email sent successfully to {0}", emailAddress) };
                }
                catch (Exception ex)
                {
                    _logger.Error("Error Sending Email", ex);
                    return new GenericActionResult<string>() { IsSuccess = false, Errors = new List<string>() { "Error occured during sending email to user" } };
                }
            }
            else
            {
                return new GenericActionResult<string>() { IsSuccess = false, Result = string.Format("No user exists with email address: {0}", emailAddress) };
            }
        }

        //TODO REFACTOR THIS METHOD
        public GenericActionResult<string> SetPassword(SetPasswordModel model)
        {
            try
            {
                var checkPasswordResult = this.PasswordValidator.ValidateAsync(model.ConfirmPassword);

                if (checkPasswordResult.Result.Succeeded)
                {
                    var removePasswordresult = this.RemovePassword(model.UserId);

                    if (removePasswordresult.Succeeded)
                    {
                        var result = this.AddPassword(model.UserId, model.ConfirmPassword);

                        if (result.Succeeded)
                        {
                            return new GenericActionResult<string>
                            {
                                IsSuccess = true,
                                Result = "Password updated sucessfully!"
                            };
                        }
                        else
                        {
                            return new GenericActionResult<string>
                            {
                                IsSuccess = true,
                                Result = null,
                                Errors = result.Errors
                            };
                        }
                    }
                    else
                    {
                        return new GenericActionResult<string>()
                        {
                            Errors = checkPasswordResult.Result.Errors,
                            IsSuccess = false
                        };
                    }
                }
                else
                {
                    return new GenericActionResult<string>()
                    {
                        Errors = checkPasswordResult.Result.Errors,
                        IsSuccess = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Updated password failed", ex);
                return new GenericActionResult<string>()
                {
                    Errors = new List<string>() { "Password updates failed!" },
                    IsSuccess = false
                };
            }
        }

        public GenericActionResult<string> UpdateUser(EditUserModel userModel)
        {
            try
            {
                var model = this.FindById(userModel.UserId);

                model.FirstName = userModel.FirstName;
                model.LastName = userModel.LastName;
                model.PhoneNumber = userModel.MobileNumber;

                var result = this.Update(model);

                if (result.Succeeded)
                {
                    return new GenericActionResult<string>()
                    {
                        Result = "User updated sucessfully",
                        IsSuccess = true,
                    };
                }
                else
                {
                    return new GenericActionResult<string>()
                    {
                        Result = null,
                        IsSuccess = false,
                        Errors = result.Errors
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred updating the user", ex);

                return new GenericActionResult<string>()
                {
                    Result = null,
                    IsSuccess = false,
                    Errors = new List<string>() { "Error occured updating the user,please contact your system administrator." }
                };
            }
        }
    }
}
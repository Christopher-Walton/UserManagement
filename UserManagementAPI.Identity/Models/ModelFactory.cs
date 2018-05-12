using Microsoft.AspNet.Identity.EntityFramework;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Infrastructure;

namespace UserManagementAPI.Identity.Models
{
  

    public class ModelFactory : IModelFactory
    {
        //private readonly IUserManager _appUserManager;

        public ModelFactory(ApplicationUserManager appUserManager)
        {
            //_appUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                MobileNumber = appUser.PhoneNumber
                
                //TODO
                //Addition of roles & claims
            };
        }

        public UserDataModel CreateUserDataModel(ApplicationUser appUser)
        {
            return new UserDataModel
            {
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                UserName = appUser.UserName,
                UserId = appUser.Id,
                MobileNumber = appUser.PhoneNumber,
            };
        }

        public UserAuthorizationInfo CreateUserAuthorizationInfo(ApplicationUser appUser)
        {
            return new UserAuthorizationInfo()
            {
                UserId = appUser.Id,
                UserName = appUser.UserName,
                //Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result.ToList(),
                //Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result,
                //ClaimsIdentity = _AppUserManager.CreateIdentity(appUser, "Token")
            };
        }

        public RoleReturnModel Create(IdentityRole role)
        {
            return new RoleReturnModel()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public UserAuthorizationInfo CreateUserAuthorizationInfo(UserReturnModel userReturnModel)
        {
            return new UserAuthorizationInfo()
            {
                UserId = userReturnModel.Id,
                UserName = userReturnModel.UserName,
                Roles = userReturnModel.Roles,
            };
        }
    }
}
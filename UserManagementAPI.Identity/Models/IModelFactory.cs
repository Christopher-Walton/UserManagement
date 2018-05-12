using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Infrastructure;

namespace UserManagementAPI.Identity.Models
{
    public interface IModelFactory
    {
        RoleReturnModel Create(IdentityRole role);

        UserDataModel CreateUserDataModel(ApplicationUser appUser);

        UserReturnModel Create(ApplicationUser appUser);

        UserAuthorizationInfo CreateUserAuthorizationInfo(ApplicationUser appUser);

        UserAuthorizationInfo CreateUserAuthorizationInfo(UserReturnModel userReturnModel);
    }
}

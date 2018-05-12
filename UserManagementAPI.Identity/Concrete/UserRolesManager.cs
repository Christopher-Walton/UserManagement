using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Infrastructure;
using UserManagementAPI.Identity.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;

namespace UserManagementAPI.Identity.Concrete
{
    public class UserRoleManager : ApplicationRoleManager, IUserRoleManager
    {
        private IModelFactory _modelFactory;

        public UserRoleManager(IModelFactory modelFactory, IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
            _modelFactory = modelFactory;
        }

        public GenericActionResult<RoleReturnModel> GetRoleById(string id)
        {
            var role = this.FindById(id);

            if (role != null)
                return new GenericActionResult<RoleReturnModel> { Result = _modelFactory.Create(role), IsSuccess = true, Errors = null };
            else
                return new GenericActionResult<RoleReturnModel> { Result = null, IsSuccess = false, Errors = new List<string> { "Role not found" } };
        }

        public GenericActionResult<RoleReturnModel> GetRoleByName(string name)
        {
            var role = this.FindByName(name);

            if (role != null)
                return new GenericActionResult<RoleReturnModel> { Result = _modelFactory.Create(role), IsSuccess = true, Errors = null };
            else
                return new GenericActionResult<RoleReturnModel> { Result = null, IsSuccess = false, Errors = new List<string> { "Role not found" } };
        }

        public IEnumerable<RoleReturnModel> GetAllRoles()
        {
            var list = this.Roles.ToList();

            var usersModels = list.Select(r => _modelFactory.Create(r));

            return usersModels;
        }

        public GenericActionResult<string> DeleteRole(string roleId)
        {
            var role = this.FindById(roleId);

            if (role != null)
            {
                var result = this.Delete(role);

                if (result.Succeeded)
                    return new GenericActionResult<string> { IsSuccess = true, Errors = null, Result = "Role deleted successfully" };
                else
                    return new GenericActionResult<string> { IsSuccess = false, Errors = result.Errors, Result = null };
            }
            else
            {
                return new GenericActionResult<string> { IsSuccess = false, Errors = new List<string> { "Role does not exist" } };
            }
        }

        public GenericActionResult<RoleReturnModel> CreateRole(CreateRoleBindingModel role)
        {
            var identityRole = new IdentityRole
            {
                Name = role.Name
            };

            var result = this.Create(identityRole);

            if (result.Succeeded)
                return new GenericActionResult<RoleReturnModel> { IsSuccess = true, Result = _modelFactory.Create(identityRole) };
            else
                return new GenericActionResult<RoleReturnModel> { IsSuccess = false, Errors = result.Errors };
        }

        public IEnumerable<string> GetRoleNames(List<string> roleIds)
        {
            var roleNames = new List<string>();
            foreach (var roleId in roleIds)
            {
                var role = this.FindById(roleId);

                if (role != null)
                    roleNames.Add(role.Name);
            }

            return roleNames;
        }
    }
}
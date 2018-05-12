using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;

namespace UserManagementAPI.Identity.Concrete
{
    public class RoleManagement : IRoleManagement
    {
        private readonly IUserRoleManager _userRoleManager;
        private readonly IUserManager _UserManager;

        public RoleManagement(IUserRoleManager userRoleManager, IUserManager UserManager)//,ApplicationUserManager userManager,
        {
            _userRoleManager = userRoleManager;
            _UserManager = UserManager;
        }

        public GenericActionResult<RoleReturnModel> GetRoleById(string Id)
        {
            var role = _userRoleManager.GetRoleById(Id);

            return role;
        }

        public GenericActionResult<RoleReturnModel> GetRoleByName(string name)
        {
            return _userRoleManager.GetRoleByName(name);
        }

        public IEnumerable<RoleReturnModel> GetAllRoles()
        {
            return _userRoleManager.GetAllRoles();
        }

        public GenericActionResult<string> DeleteRole(string roleId)
        {
            return _userRoleManager.DeleteRole(roleId);
        }

        public GenericActionResult<RoleReturnModel> CreateRole(CreateRoleBindingModel role)
        {
            return _userRoleManager.CreateRole(role);
        }

        public GenericActionResult<IEnumerable<RoleReturnModel>> AssignRolesToUser(string userId, IEnumerable<string> rolesToAssign)
        {
            var user = _UserManager.GetUserById(userId);

            if (user == null)
            {
                return new GenericActionResult<IEnumerable<RoleReturnModel>> { Errors = new List<string> { "User not found" }, IsSuccess = false };
            }

            var currentRoles = _UserManager.GetUserRoles(user.Id);

            var rolesNotExisting = rolesToAssign.Except(_userRoleManager.GetAllRoles().Select(c => c.Name)).ToArray();

            if (rolesNotExisting.Count() > 0)
            {
                return new GenericActionResult<IEnumerable<RoleReturnModel>>
                {
                    Errors = new List<string> { string.Format("Roles '{0}' does not exist in the system", string.Join(",", rolesNotExisting)) },
                    IsSuccess = false
                };
            }

            var removeResult = _UserManager.RemoveUserFromRoles(user.Id, currentRoles.ToArray());

            if (!removeResult.IsSuccess)
            {
                return new GenericActionResult<IEnumerable<RoleReturnModel>> { IsSuccess = false, Errors = removeResult.Errors };
            }

            //TODO - SHOULD BE REFACTORED
            //PLEASE REFACTOR
            var finalRolesAddition = rolesToAssign.ToList();

            //Always Add Default System Role
            //ENSURE THAT "User" role is always added as it is the default user role of the system
            //get the default user role and assign it
            var defaultUserRole = ConfigurationManager.AppSettings["DefaultUserRole"];

            finalRolesAddition.Add(defaultUserRole);

            //remove duplicates if necessary
            finalRolesAddition = finalRolesAddition.GroupBy(c => c).Select(g => g.First()).ToList();

            var addRolesResult = _UserManager.AddUserToRoles(user.Id, finalRolesAddition);

            if (!addRolesResult.IsSuccess)
            {
                return new GenericActionResult<IEnumerable<RoleReturnModel>> { IsSuccess = false, Errors = removeResult.Errors };
            }
            else
            {
                return new GenericActionResult<IEnumerable<RoleReturnModel>> { IsSuccess = true };
            }
        }
    }
}
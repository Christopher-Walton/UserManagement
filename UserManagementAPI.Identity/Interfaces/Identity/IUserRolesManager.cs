using System.Collections.Generic;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;

namespace UserManagementAPI.Identity.UserManagementInterfaces
{
    public interface IUserRoleManager
    {
        GenericActionResult<RoleReturnModel> CreateRole(CreateRoleBindingModel role);

        GenericActionResult<string> DeleteRole(string roleId);

        IEnumerable<RoleReturnModel> GetAllRoles();

        GenericActionResult<RoleReturnModel> GetRoleById(string id);

        GenericActionResult<RoleReturnModel> GetRoleByName(string name);

        IEnumerable<string> GetRoleNames(List<string> RoleId);
    }
}
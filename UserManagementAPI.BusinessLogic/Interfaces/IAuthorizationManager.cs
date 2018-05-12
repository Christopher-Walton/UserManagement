using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;

namespace UserManagementAPI.BusinessLogic
{

    public interface IAuthorizationManager
    {
        GenericActionResult<string> CreateAuthorizationToken(string userName, string password);

        UserAuthorizationInfo GetAuthorizationInformation(string token);

        GenericActionResult<UserDataModel> AuthenticateUser(string userName, string password);
    }
}

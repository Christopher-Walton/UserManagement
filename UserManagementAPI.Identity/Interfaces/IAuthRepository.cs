using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Identity.Infrastructure.Authentication;

namespace UserManagementAPI.Identity.UserManagementInterfaces
{
    public interface IAuthenticationRepository
    { 
        string CreateToken(string serializedDetails);

        AuthenticationInformation GetAuthorizationInformation(string token);
    }
}

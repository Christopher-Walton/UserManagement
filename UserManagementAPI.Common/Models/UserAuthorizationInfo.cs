using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Common.Models
{
    public class UserAuthorizationInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IList<string> Roles { get; set; }
        
        //public IList<System.Security.Claims.Claim> Claims { get; set; }
        //public System.Security.Claims.ClaimsIdentity ClaimsIdentity { get; set; }
    }
}

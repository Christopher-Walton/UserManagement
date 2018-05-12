using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Domain
{
    public class AuthenticationInformation
    {
        public Guid TokenId {get;set;}
        public string UserIdentityDetails { get; set; }
    }
}

using System.Collections.Generic;
using UserManagementAPI.Common.Models;

namespace UserManagementAPI.Common.IdentityHelpers
{
    public class RoleActionResult
    {
        public RoleReturnModel Role { get; set; }
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class UserActionResult
    {
        public UserReturnModel User { get; set; }

        public bool Successful
        {
            get;
            set;
        }

        public IEnumerable<string> Errors
        {
            get;
            set;
        }
    }
}
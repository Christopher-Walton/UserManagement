using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace UserManagementAPI.Identity.WebAPI.CustomAuthorization
{
    public class CustomUserPrincipal : GenericPrincipal
    {
        public CustomUserPrincipal(IIdentity identity, string[] roles,string userId)
        : base(identity, roles)
        {
            this.UserId = userId;
        }

        private readonly string UserId;


        public string GetUserId()
        {
            return UserId;
        }
        //public MyUser UserDetails {get; set;}
    }
}
using System;

namespace UserManagementAPI.Identity.WebAPI.CustomAuthorization
{
    public class AuthorizationActionAttribute : Attribute
    {
        public string Roles { get; set; }

        public AuthorizationActionAttribute()
        {
        }

        public AuthorizationActionAttribute(string roles)
        {
            this.Roles = roles;
        }
    }
}
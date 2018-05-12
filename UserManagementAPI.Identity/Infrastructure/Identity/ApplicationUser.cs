using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Claims;

namespace UserManagementAPI.Identity.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public ClaimsIdentity GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = manager.CreateIdentity(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
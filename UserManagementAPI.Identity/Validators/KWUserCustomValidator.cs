using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using UserManagementAPI.Identity.Infrastructure;

namespace UserManagementAPI.Identity.Validators
{
    public class CustomUserValidator : UserValidator<ApplicationUser>
    {
        public CustomUserValidator(ApplicationUserManager appUserManager)
            : base(appUserManager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            return result;
        }

        //List<string> _allowedEmailDomains = new List<string> { "outlook.com", "hotmail.com", "gmail.com", "yahoo.com" };
        //public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        //{
        //    IdentityResult result = await base.ValidateAsync(user);

        //    var emailDomain = user.Email.Split('@')[1];

        //    if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
        //    {
        //        var errors = result.Errors.ToList();

        //        errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

        //        result = new IdentityResult(errors);
        //    }

        //    return result;
        //}
    }
}
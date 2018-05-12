using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using UserManagementAPI.Identity.Services;
using UserManagementAPI.Identity.Validators;

namespace UserManagementAPI.Identity.Infrastructure
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            var provider = new DpapiDataProtectionProvider("UserManagementAPI");
            this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("Confirmation"));

            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            //this should be configurable
            this.UserValidator = new CustomUserValidator(this)
            {
                RequireUniqueEmail = true
            };

            //this should be configurable
            this.PasswordValidator = new CustomPasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = true,
            };

            this.EmailService = new EmailServiceKW();
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Common.Models
{
    public class CreateUserBindingModel
    {
        [Required(ErrorMessage = "The email address is required",AllowEmptyStrings=false)]
        [EmailAddress(ErrorMessage = "This must be a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings=false)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings=false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{4}",ErrorMessage="Format of the mobile number must be ###-###-####")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Common.Models
{
    public class CompanyConsigneeBindingModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The company name is required")]
        [Display(Name="Company Name")]
        public string CompanyName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The company address is required")]
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The company phone number is required")]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{4}", ErrorMessage = "Format of the mobile number must be ###-###-####")]
        [Display(Name = "Company Phone")]
        public string CompanyPhone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The company email address is required")]
        [EmailAddress(ErrorMessage = "Must be a valid email address")]
        [Display(Name = "Company Email")]
        public string CompanyEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The company trn is required")]
        [StringLength(12)]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{3}", ErrorMessage = "TRN must be in the format: ###-###-###")]
        [Display(Name = "Company TRN")]
        public string CompanyTRN { get; set; }

     }

    public class WISUsers
    {
        public string UserId { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "The customer code is required.")]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "The WIS Consignee name is required.")]
        public string WISConsignee { get; set; }

        [Required(ErrorMessage = "The WIS Consignee address is required.")]
        public string WISAddress { get; set; }
    }
}
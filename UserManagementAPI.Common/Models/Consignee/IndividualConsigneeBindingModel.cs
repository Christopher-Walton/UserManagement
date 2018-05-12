using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Common.Models.Consignee
{
    public class IndividualConsigneeBindingModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{3}", ErrorMessage = "TRN must be in the format: ###-###-###")]
        public string TRN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public string CustomerCode { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Common.Models.Consignee;

namespace UserManagementAPI.Common.Models
{
    public class IndividualRegistration
    {
      public CreateUserBindingModel PrimaryUser { get; set; }

      //public IndividualConsigneeBindingModel Consignee { get; set; }

      [Required(AllowEmptyStrings=false,ErrorMessage="TRN is required")]
      [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{3}", ErrorMessage = "TRN must be in the format: ###-###-###")]
      public string TRN { get; set; }

      [Required(AllowEmptyStrings = false, ErrorMessage = "Address name is required")]
      public string Address { get; set; }
    }


    public class PayAndGoRegistration: IndividualRegistration
    {
        [Required(ErrorMessage="You must check this checkbox in order to agree to the terms of the pay and go service.")]
        public bool AgreeToPayandGoConditions { get; set; }

        //public byte[] ScannedImage { get; set; }

    }

}

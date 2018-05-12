using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UserManagementAPI.Common.Models
{
    public class EditUserModel
    {
        public EditUserModel()
        {
          
        }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{4}", ErrorMessage = "Format of the mobile number must be ###-###-####")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        
        //TODO
        public string UserId { get; set; }
    }


    public class UserReturnModel
    {
        //public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        


        public IList<string> Roles { get; set; }
        //public IList<System.Security.Claims.Claim> Claims { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }
    }

    public class UserDataModel
    {       
        public bool IsAuthenticated { get; set; }
        public string AuthorizationToken { get; set; }
        
        public string UserId { get; set; }
        public string UserName { get; set; }
        
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public List<string> CustomerNames { get; set; }
        public List<string> Roles { get; set; }
    }
}

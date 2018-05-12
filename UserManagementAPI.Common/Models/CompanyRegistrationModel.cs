using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Common.Models
{
    public class CompanyRegistrationModel
    {
      public CreateUserBindingModel PrimaryUser { get; set; }
      public CompanyConsigneeBindingModel Company { get; set; }
    }
}

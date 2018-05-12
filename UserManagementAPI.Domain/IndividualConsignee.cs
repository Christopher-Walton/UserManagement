using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Domain
{
    public class IndividualConsignee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerCode { get; set; }
        public string TRN { get; set; }

        //ISO Standard Address
        public string Address { get; set; }
    }
}

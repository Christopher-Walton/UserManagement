using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Identity.Domain
{
    public class CompanyConsignee
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public long   CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyTRN { get; set; }

        //WIS Data Is Here
        public string CustomerCode { get; set; }
        public string WISConsignee { get; set; }
        public string WISAddress { get; set; }

    }
}

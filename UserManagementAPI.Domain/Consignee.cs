using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Domain
{
    public class CompanyConsignee
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public long   CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyTRN { get; set; }
             
    }
}

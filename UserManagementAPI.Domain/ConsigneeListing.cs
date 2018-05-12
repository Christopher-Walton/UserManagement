using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Domain
{
    public class ConsigneeListing
    {
        public IEnumerable<CompanyConsignee> CompanyConsignees { get; set; }
        public IEnumerable<IndividualConsignee> IndividualConsignees { get; set; }

        public List<string> GetAllConsigneeNames()
        {
            var names = new List<string>();

            names.AddRange(CompanyConsignees.Select(c => c.CompanyName));
            names.AddRange(IndividualConsignees.Select(c => string.Format("{0} {1}",c.FirstName,c.LastName)));

            return names;


        }
    }
}

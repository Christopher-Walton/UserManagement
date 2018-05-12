using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.BusinessLogic
{
    public interface IValidationHelper
    {
        bool CheckForExistingCompany(string CompanyTRN);

        bool CheckForExistingIndividual(string individualTRN);
    }

}

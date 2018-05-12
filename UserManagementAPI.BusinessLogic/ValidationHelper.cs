using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.DataAccess;

namespace UserManagementAPI.BusinessLogic
{
    public class ValidationHelper : IValidationHelper
    {
        private IDataAccess _dataAccess;

        public ValidationHelper(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public bool CheckForExistingCompany(string CompanyTRN)
        {
            var company = _dataAccess.GetCompanyConsignee(CompanyTRN);

            if (company == null)
                return false;
            else
                return true;
        }

        public bool CheckForExistingIndividual(string individualTRN)
        {
            var individual = _dataAccess.GetIndividualConsignee(individualTRN);

            if (individual == null)
                return false;
            else
                return true;
        }
    }
}

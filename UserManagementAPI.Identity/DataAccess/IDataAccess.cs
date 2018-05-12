using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Infrastructure.Authentication;

namespace UserManagementAPI.Identity.DataAccess
{
    public interface IDataAccess
    {
        IEnumerable<WISUserInfo> GetWISUSers();

        #region Authorization/Authentications

        string CreateToken(string serializedDetails);

        AuthenticationInformation GetAuthorizationInformation(string token);

        #endregion Authorization/Authentications

        #region Consignee

        int CreateCompanyConsignee(CompanyConsigneeBindingModel companyConsignee);

        int CreateIndividualConsignee(IndividualConsigneeBindingModel individualConsigneeModel);

        void CreateIndividualConsigneeRepresentative(string userId, int individualConsigneeId);

        void CreateCompanyConsigneeRepresentative(string userId,int companyConsigneeId);
        
        #endregion Consignee
    }
}
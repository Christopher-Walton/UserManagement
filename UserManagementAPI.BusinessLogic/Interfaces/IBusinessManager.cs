using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Common.Models.Consignee;

namespace UserManagementAPI.BusinessLogic
{
    public interface IBusinessManager
    {
        GenericActionResult<RegistrationReturnModel<CompanyConsigneeBindingModel>> RegisterCompanyWithPrincipal(CreateUserBindingModel principalUser, CompanyConsigneeBindingModel companyConsignee);

        GenericActionResult<RegistrationReturnModel<IndividualConsigneeBindingModel>> RegisterIndividual(CreateUserBindingModel principalUser, IndividualConsigneeBindingModel indivdualConsignee);
    }
}
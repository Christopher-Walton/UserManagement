using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using UserManagementAPI.Common.ActionResults;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Common.Models.Consignee;
using UserManagementAPI.DataAccess;
using UserManagementAPI.Identity.UserManagementInterfaces;

namespace UserManagementAPI.BusinessLogic
{
    public class BusinessManager : IBusinessManager, IAuthorizationManager
    {
        private IDataAccess _dataAccess;
        private IUserManagement _userManagement;
        private IRoleManagement _roleManagement;
        private IAuthenticationManagement _authenticationManagement;
        private readonly IValidationHelper _validationHelper;
        //private IModelFactory _modelFactory;

        public BusinessManager(IDataAccess dataAccess, IUserManagement userManagement, IAuthenticationManagement authenticationManagement,
            IValidationHelper validationHelper, IRoleManagement roleManagement)//,IModelFactory modelFactory)
        {
            _dataAccess = dataAccess;
            _userManagement = userManagement;
            _authenticationManagement = authenticationManagement;
            _roleManagement = roleManagement;
            _validationHelper = validationHelper;
            //_modelFactory = modelFactory;
        }

        public GenericActionResult<RegistrationReturnModel<CompanyConsigneeBindingModel>> RegisterCompanyWithPrincipal(CreateUserBindingModel principalUser, CompanyConsigneeBindingModel companyConsignee)
        {
            bool companyAlreadyExists = _validationHelper.CheckForExistingCompany(companyConsignee.CompanyTRN);

            if (companyAlreadyExists)
            {
                return new GenericActionResult<RegistrationReturnModel<CompanyConsigneeBindingModel>>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Company already registered, duplicate TRN" }
                };
            }

            var createUserResult = _userManagement.CreateUser(principalUser);

            string userId = null;

            if (createUserResult.IsSuccess)
            {
                userId = createUserResult.Result.Id;

                AssignUserToDefaultRoles(userId);
            }
            else
                return new GenericActionResult<RegistrationReturnModel<CompanyConsigneeBindingModel>>() { IsSuccess = false, Errors = createUserResult.Errors };

            //create and get company id to create a link between created user
            int? companyId = _dataAccess.CreateCompanyConsignee(companyConsignee);

            //if no company Id was returned, it means an error occured during creation of the user
            //in that case roll back the created user transactions
            if (!companyId.HasValue)
            {
                //TODO could use transactions here
                //reverse the created user by deleting it
                _userManagement.DeleteUser(createUserResult.Result.UserName);

                return new GenericActionResult<RegistrationReturnModel<CompanyConsigneeBindingModel>>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Error completing registration,please try again later." }
                };
            }

            //create a link record between the company and the user
            _dataAccess.CreateCompanyConsigneeRepresentative(userId, companyId.Value);

            //return new information on user & company
            return new GenericActionResult<RegistrationReturnModel<CompanyConsigneeBindingModel>>()
            {
                IsSuccess = true,
                Errors = null,
                Result = new RegistrationReturnModel<CompanyConsigneeBindingModel>()
                {
                    UserModel = createUserResult.Result,
                    ConsigneeModel = companyConsignee
                }
            };
        }

        public GenericActionResult<RegistrationReturnModel<IndividualConsigneeBindingModel>> RegisterIndividual(CreateUserBindingModel principalUser,
                                                                                                                IndividualConsigneeBindingModel individualConsignee)
        {
            //validate individual doesn't exist first
            bool personAlreadyExists = _validationHelper.CheckForExistingIndividual(individualConsignee.TRN);

            if (personAlreadyExists)
                return new GenericActionResult<RegistrationReturnModel<IndividualConsigneeBindingModel>>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Individual already registered, duplicate TRN" }
                };

            var userCreateResult = _userManagement.CreateUser(principalUser);

            string userId = null;

            if (userCreateResult.IsSuccess)
            {
                userId = userCreateResult.Result.Id;
                AssignUserToDefaultRoles(userId);
            }
            else
            {
                return new GenericActionResult<RegistrationReturnModel<IndividualConsigneeBindingModel>>()
                {
                    Errors = userCreateResult.Errors,
                    IsSuccess = false,
                    Result = null
                };
            }

            int? indivdualConsigneeId = _dataAccess.CreateIndividualConsignee(individualConsignee);

            //if an id wasn't returned, it means an error occurred during the creation of the consignee record
            //therefore roll back all that came before it.
            if (!indivdualConsigneeId.HasValue)
            {
                //reverse the created user by deleting it
                _userManagement.DeleteUser(userCreateResult.Result.UserName);

                return new GenericActionResult<RegistrationReturnModel<IndividualConsigneeBindingModel>>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Error completing registration,please try again later." }
                };
            }

            //create link between individual and consignee
            _dataAccess.CreateIndividualConsigneeRepresentative(userId, indivdualConsigneeId.Value);

            //return new information on user & company
            return new GenericActionResult<RegistrationReturnModel<IndividualConsigneeBindingModel>>()
            {
                IsSuccess = true,
                Result = new RegistrationReturnModel<IndividualConsigneeBindingModel>
                {
                    ConsigneeModel = individualConsignee,
                    UserModel = userCreateResult.Result
                },
                Errors = null
            };
        }

        public GenericActionResult<string> CreateAuthorizationToken(string userName, string password)
        {
            var authenticateUserResult = _authenticationManagement.AuthenticateUser(userName, password);

            if (!authenticateUserResult.IsSuccess)
                return new GenericActionResult<string>()
                {
                    IsSuccess = false,
                    Result = null,
                    Errors = authenticateUserResult.Errors
                };

            //get all information on person,roles & claims
            var user = _userManagement.FindByUserName(userName);

            var userAuthorizationInfo = new UserAuthorizationInfo()
            {
                UserName = authenticateUserResult.Result.UserName,
                ExpiryDate = DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["tokenExpirationInMinutes"])),
                UserId = user.Id,
                Roles = user.Roles
            };

            //serialize object for storing in database
            var serializedAuthorizationInfo = JsonConvert.SerializeObject(userAuthorizationInfo);

            var authToken = _dataAccess.CreateToken(serializedAuthorizationInfo);

            var authenticationResult = new GenericActionResult<string>() { Errors = null, IsSuccess = true, Result = authToken };

            return authenticationResult;
        }

        public UserAuthorizationInfo GetAuthorizationInformation(string token)
        {
            var authenticationInfo = _dataAccess.GetAuthorizationInformation(token);

            if (authenticationInfo == null)
                return null;
            else
                return JsonConvert.DeserializeObject<UserAuthorizationInfo>(authenticationInfo.UserIdentityDetails);
        }

        public GenericActionResult<UserDataModel> AuthenticateUser(string userName, string password)
        {
            var authenticateUser = _authenticationManagement.AuthenticateUser(userName, password);

            if (authenticateUser.IsSuccess)
            {
                //get all consignees which the user represents
                var consignees = _dataAccess.GetConsigneeByUser(authenticateUser.Result.UserId);

                //get all the customers which this user represents
                authenticateUser.Result.CustomerNames = consignees.GetAllConsigneeNames();

                //get the authorization token for the user //TO verify success first
                var authorizationToken = CreateAuthorizationToken(userName, password);
                authenticateUser.Result.AuthorizationToken = authorizationToken.Result;
            }

            return authenticateUser;
        }

        //TODO REFACTOR
        private GenericActionResult<IEnumerable<RoleReturnModel>> AssignUserToDefaultRoles(string userId)
        {
            var result = _roleManagement.AssignRolesToUser(userId, new List<string>().ToArray());

            return result;
        }
    }
}
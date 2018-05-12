using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserManagementAPI.BusinessLogic;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Common.Models.Consignee;
using UserManagementAPI.Identity.UserManagementInterfaces;
using UserManagementAPI.Identity.WebApi.CustomValidation;
using UserManagementAPI.Identity.WebAPI.Controllers;
using NLog;

namespace UserManagementAPI.Identity.WebApi.Controllers
{   
    [RoutePrefix("api/Registration")]
    public class RegistrationController : BaseApiController
    {
        private readonly IBusinessManager _businessManager;
        private readonly IUserManagement _userManagement;
        private readonly IUtilityManager _utilityManager;

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public RegistrationController(IBusinessManager businessManager,IUserManagement userManagement,IUtilityManager utilityManager)
        {
            _businessManager = businessManager;
            _userManagement = userManagement;
            _utilityManager = utilityManager;
        }

        [Route("RegisterCompany")]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public IHttpActionResult CompanySignUp(CompanyRegistrationModel companyRegistrationModel)
        {
            var registerCompanyWithPrincipalUserResult = _businessManager.RegisterCompanyWithPrincipal(companyRegistrationModel.PrimaryUser,companyRegistrationModel.Company);

            if (!registerCompanyWithPrincipalUserResult.IsSuccess)
                return GetErrorResult(registerCompanyWithPrincipalUserResult);

            var userId = registerCompanyWithPrincipalUserResult.Result.UserModel.Id;
            var userName = registerCompanyWithPrincipalUserResult.Result.UserModel.UserName;

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = userId }));

            return Created(locationHeader, registerCompanyWithPrincipalUserResult.Result.UserModel);
        }

        [Route("RegisterIndividual")]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public IHttpActionResult IndividualSignUp(IndividualRegistration individualRegistrationModel)
        {

            var individualConsigneeModel = new IndividualConsigneeBindingModel() { 
                Address = individualRegistrationModel.Address,
                TRN = individualRegistrationModel.TRN,
                FirstName = individualRegistrationModel.PrimaryUser.FirstName,
                LastName = individualRegistrationModel.PrimaryUser.LastName,
            };

            var registerIndividualResult = _businessManager.RegisterIndividual(individualRegistrationModel.PrimaryUser, individualConsigneeModel);
            
           if (!registerIndividualResult.IsSuccess)
               return GetErrorResult(registerIndividualResult);

           var userId = registerIndividualResult.Result.UserModel.Id;

           Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = userId }));

           return Created(locationHeader, registerIndividualResult.Result.UserModel);
        }

        [Route("PayAndGoIndividual")]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public IHttpActionResult PayAndGoIndividual(PayAndGoRegistration payAndGoRegistration)
        {
            //save to database here the new images
            //save to the database


            //send email to customer service here
            var result = _utilityManager.SendEmailToUser("christopher.walton@BELjm.com","Pay and Go Sign Up",string.Format("The following user has signed up for Pay and Go: <br/><br/>"+
                "First Name : {0}<br/> LastName : {1}<br/> TRN : {2}<br/>Address : {3}<br/>Email Address : {4} <br/>UserName : {5}<br/>Password : {6}<br/>Mobile Number : {7}<br/>",
                payAndGoRegistration.PrimaryUser.FirstName,payAndGoRegistration.PrimaryUser.LastName,payAndGoRegistration.TRN,payAndGoRegistration.Address,
                payAndGoRegistration.PrimaryUser.Email,payAndGoRegistration.PrimaryUser.Username,payAndGoRegistration.PrimaryUser.Password,payAndGoRegistration.PrimaryUser.MobileNumber)
                       
            );

            if (result.IsSuccess)
            {
                _logger.Debug("test email sent");
                return Ok("Pay and Go Notification Sent");
            }
            else
                return GetErrorResult(result);
         
        }

       

    }
}

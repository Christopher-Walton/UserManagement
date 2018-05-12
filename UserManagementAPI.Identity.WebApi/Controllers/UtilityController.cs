using System.Web.Http;
using UserManagementAPI.BusinessLogic;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.WebApi.CustomValidation;
using UserManagementAPI.Identity.WebAPI.Controllers;

namespace UserManagementAPI.Identity.WebApi.Controllers
{
    [RoutePrefix("api/Utility")]
    public class UtilityController : BaseApiController
    {
        private readonly IUtilityManager _utilityManager;

        public UtilityController(IUtilityManager utilityManager)
        {
            _utilityManager = utilityManager;
        }

        [Route("SendEmail")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public IHttpActionResult SendEmail(EmailModel email)
        {
            var sendEmailResult = _utilityManager.SendEmailToUser(email.EmailAddress, email.Subject, email.Body);

            if (!sendEmailResult.IsSuccess)
                return GetErrorResult(sendEmailResult);
            else
            {
                return Ok(sendEmailResult.Result);
            }
        }
    }
}
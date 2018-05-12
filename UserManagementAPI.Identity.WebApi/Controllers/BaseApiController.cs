using System.Collections.Generic;
using System.Web.Http;
using UserManagementAPI.Common.ActionResults;

namespace UserManagementAPI.Identity.WebAPI.Controllers
{
    public class ErrorResult {
        public List<string> ErrorMessages { get; set; }
    }

    public class BaseApiController : ApiController
    {
        protected IHttpActionResult GetErrorResult(IGenericActionResult result)
        {
            var errorResult = new ErrorResult() { };

            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.IsSuccess)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        //errorResult.ErrorMessages.Add(error);
                        ModelState.AddModelError("ErrorMessage", error);
                    }
                }
                
                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            //if it got here something definitely went wrong, would have to figure out what it could be
            return null;
        }
    }
}
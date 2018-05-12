using Newtonsoft.Json;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace UserManagement.Identity.UI.CustomAuthorization
{
    //TODO CHECKS THIS FUNCTION
    //VERY CONFUSED BY THIS NECESSITY OF THIS ATTRIBUTE ESPECIALLY THIS SECTION
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthenticated = base.AuthorizeCore(httpContext);

            if (isAuthenticated)
            {
                string cookieName = FormsAuthentication.FormsCookieName;
                if (!httpContext.User.Identity.IsAuthenticated ||
                    httpContext.Request.Cookies == null ||
                    httpContext.Request.Cookies[cookieName] == null)
                {
                    return false;
                }

                var authCookie = httpContext.Request.Cookies[cookieName];
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                //read the user data portion of the authentication ticket
                UserData authDetails = JsonConvert.DeserializeObject<UserData>(authTicket.UserData);

                 CustomUserPrincipal principal = new CustomUserPrincipal(new GenericIdentity(authTicket.Name),
                                                    authDetails.Roles.ToArray(), authTicket.Name,
                                                    authDetails.WebServiceToken, authDetails.UserId);

                httpContext.User = principal;
            }

            return isAuthenticated;
        }

        //Called when access is denied
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //User isn't logged in
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "SignIn" })
                );
            }

            //User is logged in but has no access
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Unauthorized" })
                );
            }
        }
    }
}
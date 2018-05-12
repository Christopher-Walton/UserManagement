using Newtonsoft.Json;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using UserManagement.Identity.UI.CustomAuthorization;

namespace UserManagement.Identity.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                UserData authDetails = JsonConvert.DeserializeObject<UserData>(authTicket.UserData);

                CustomUserPrincipal principal = new CustomUserPrincipal(new GenericIdentity(authTicket.Name),
                                        authDetails.Roles.ToArray(), authTicket.Name, 
                                        authDetails.WebServiceToken, authDetails.UserId);

                Context.User = principal;
            }
        }
    }
}
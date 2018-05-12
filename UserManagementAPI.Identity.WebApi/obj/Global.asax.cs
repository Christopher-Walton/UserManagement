using System;
using System.Web.Http;
using UserManagementAPI.Identity.WebAPI.App_Start;

namespace UserManagementAPI.Identity.WebAPI
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //GlobalConfiguration.Configuration.Routes.Add("default", new HttpRoute("{controller}"));
            //WebApiConfig.Register(GlobalConfiguration.Configuration)

            GlobalConfiguration.Configure(WebApiConfig.Register);
           
            //GlobalConfiguration.Configuration.DependencyResolver = new NinjectHttpDependencyResolver(NinjectWebCommon.C)
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}
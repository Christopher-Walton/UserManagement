[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UserManagementAPI.Identity.WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(UserManagementAPI.Identity.WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace UserManagementAPI.Identity.WebApi.App_Start
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.WebApi.FilterBindingSyntax;
    using System;
    using System.Web;
    using System.Web.Http.Filters;
    using UserManagementAPI.BusinessLogic;
    using UserManagementAPI.DataAccess;
    using UserManagementAPI.Identity.Concrete;
    using UserManagementAPI.Identity.Infrastructure;
    //using UserManagementAPI.Identity.Interfaces;
    using UserManagementAPI.Identity.Models;
    using UserManagementAPI.Identity.UserManagementInterfaces;
    using UserManagementAPI.Identity.WebAPI.CustomAuthorization;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IModelFactory>().To<ModelFactory>();
            kernel.Bind<IUserRoleManager>().To<UserRoleManager>();
            kernel.Bind<IUserManager>().To<UserManager>();
            kernel.Bind<IAuthenticationManagement>().To<AuthenticationManagement>();

            kernel.Bind<ModelFactory>().To<ModelFactory>();
            //kernel.Bind<IAuthenticationRepository>().To<AuthenticationRepository>();
            kernel.Bind<ApplicationUserManager>().To<ApplicationUserManager>();
            kernel.Bind<ApplicationDbContext>().To<ApplicationDbContext>();
            kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>().WithConstructorArgument("context", kernel.Get<ApplicationDbContext>());
            kernel.Bind<IUserManagement>().To<UserManagement>();
            kernel.Bind<IAuthorizationManager>().To<BusinessManager>();
            kernel.Bind<IBusinessManager>().To<BusinessManager>();
            kernel.Bind<IDataAccess>().To<DataAccess>();
            kernel.Bind<IValidationHelper>().To<ValidationHelper>();
            kernel.Bind<IUtilityManager>().To<UtilityManager>();
            
            kernel.BindHttpFilter(x => new AuthorizationActionFilter(x.Inject<IAuthorizationManager>()), FilterScope.Action).WhenActionMethodHas<AuthorizationActionAttribute>();

            kernel.Bind<IRoleStore<IdentityRole, string>>().To<RoleStore<IdentityRole, string, IdentityUserRole>>().WithConstructorArgument("context", kernel.Get<ApplicationDbContext>());
            kernel.Bind<RoleManager<IdentityRole>>().To<RoleManager<IdentityRole>>();
            kernel.Bind<ApplicationRoleManager>().To<ApplicationRoleManager>();
            kernel.Bind<IRoleManagement>().To<RoleManagement>();
        }
    }
}
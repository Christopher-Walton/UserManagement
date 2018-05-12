namespace UserManagementAPI.Identity.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using UserManagementAPI.Identity.Infrastructure;

    internal sealed class Configuration : DbMigrationsConfiguration<UserManagementAPI.Identity.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole, string>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole() { Name = "Administrator" });
            roleManager.Create(new IdentityRole() { Name = "User" });

            var user = new ApplicationUser()
            {
                UserName = "christopher.walton@BELjm.com",
                Email = "christopher.walton@BELjm.com",
                EmailConfirmed = true,
                FirstName = "Christopher",
                LastName = "Walton",
            };

            var userResult = manager.Create(user, "Friday1");
            
            if (userResult.Succeeded)
            {
                var userAdmin = manager.FindByEmail("christopher.walton@BELjm.com");

                manager.AddToRole(userAdmin.Id, "User");
                manager.AddToRole(userAdmin.Id, "Administrator");
            }

        }
    }
}
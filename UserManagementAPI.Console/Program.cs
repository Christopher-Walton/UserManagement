using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Identity.Infrastructure;
using Dapper;

namespace UserManagementAPI.Console
{


    internal class Program
    {

        private static IEnumerable<WISUserInfo> GetWISUSers()
        {
            var connectionStringDbRepos = ConfigurationManager.ConnectionStrings["DBRepository"].ConnectionString;

            using (var dbReposConnection = new SqlConnection(connectionStringDbRepos))
            {
                dbReposConnection.Open();

                IEnumerable<WISUserInfo> wisUserInfo = dbReposConnection.Query<WISUserInfo>("SELECT * FROM [dbo].[WISPRFILE(CustomerLoginInformation)]");

                return wisUserInfo;
            }
        }

        internal class WISUserInfo
        {
            public string CULOGN { get; set; }
            public string CUCODE { get; set; }
            public string CUPWORD { get; set; }
            public string CUNAME { get; set; }
            public string PCENCD { get; set; }
            public string ENQONLY { get; set; }
        }

        private static void Main(string[] args)
        {
            

            //  This method will be called after migrating to the latest version.
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            //var user = manager.Find("NONSWIMMER", "KINGSTON");

            //if (user != null)
            //{
            //    user.Email = "charis.pringle@BELjm.com";
            //    user.EmailConfirmed = true;

            //    manager.Update(user);
            //}

            ////var roleManager = new RoleManager<IdentityRole, string>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            ////var user = new ApplicationUser()
            ////{
            ////    UserName = "SuperUser",
            ////    Email = "christopher.walton@BELjm.com",
            ////    EmailConfirmed = true,
            ////    FirstName = "Christopher",
            ////    LastName = "Walton", //Level = 1,
            ////    JoinDate = DateTime.Now.AddMonths(-5)
            ////};

            ////manager.Create(user, "MySuperP@ssword!");

            ////seed WIS Users into database
            //try
            //{
            //    var WISUsers = GetWISUSers();
            //    foreach (var wisUser in WISUsers)
            //    {
            //        var new_user = new ApplicationUser()
            //        {
            //            UserName = wisUser.CULOGN,
            //            JoinDate = DateTime.Now,
            //            FirstName = wisUser.CUNAME,
            //            LastName = wisUser.CUNAME
            //        };

            //        manager.Create(new_user, wisUser.CUPWORD);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}
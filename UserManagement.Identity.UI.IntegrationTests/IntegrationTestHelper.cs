using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace UserManagement.Identity.UI.IntegrationTests
{
    public static class IntegrationTestHelper
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static void SignUpControllerTest_DBCleanUp()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var p = new DynamicParameters();
                p.Add("@Email","jesse5000jm@yahoo.com");

                var deleteUserResult = sqlConnection.Execute("CleanUpIndividualAccountPostTest", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

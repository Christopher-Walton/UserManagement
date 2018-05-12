using System;
using UserManagementAPI.Identity.Infrastructure.Authentication;
using UserManagementAPI.Identity.UserManagementInterfaces;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace UserManagementAPI.Identity.Concrete
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private string _AuthDBConnectionString = ConfigurationManager.ConnectionStrings["authenticationDb"].ConnectionString;

        public string CreateToken(string serializedUserDetails )
        {
            Guid token = new Guid();
            using (var sqlConnection = new SqlConnection(_AuthDBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    token = sqlConnection.ExecuteScalar<Guid>(
                       @"INSERT INTO AuthenticationInformation (UserIdentityDetails) OUTPUT Inserted.Token Values (@userClaimsInfo)", new {userClaimsInfo = serializedUserDetails },
                      commandType: CommandType.Text, commandTimeout: 60);
                  
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    //to do
                }
            }
            return token.ToString();
        }

        public AuthenticationInformation GetAuthorizationInformation(string token)
        {
            using (var sqlConnection = new SqlConnection(_AuthDBConnectionString))
            {
                sqlConnection.Open();
                IEnumerable<AuthenticationInformation> authToken = sqlConnection.Query<AuthenticationInformation>("SELECT * FROM AuthenticationInformation WHERE token=@Token",new{Token=token},
                    commandType: CommandType.Text, commandTimeout: 60);

                if (authToken != null && authToken.Count() > 0)
                    return authToken.First(); //since list should only ever contain one due to guid being unique
                else
                    return null;
            }
        }
    }
}
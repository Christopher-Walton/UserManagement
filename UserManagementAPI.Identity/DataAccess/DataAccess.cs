using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.Domain;
using UserManagementAPI.Identity.Infrastructure.Authentication;

namespace UserManagementAPI.Identity.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private string _DBConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<WISUserInfo> GetWISUSers()
        {
            var connectionStringDbRepos = ConfigurationManager.ConnectionStrings["DBRepository"].ConnectionString;

            using (var dbReposConnection = new SqlConnection(connectionStringDbRepos))
            {
                dbReposConnection.Open();

                IEnumerable<WISUserInfo> wisUserInfo = dbReposConnection.Query<WISUserInfo>("SELECT * FROM [dbo].[WISPRFILE(CustomerLoginInformation)]");

                return wisUserInfo;
            }
        }

        public string CreateToken(string serializedDetails)
        {
            throw new System.NotImplementedException();
        }

        public AuthenticationInformation GetAuthorizationInformation(string token)
        {
            throw new System.NotImplementedException();
        }

        #region Consignee and Consignee Links

        public int CreateIndividualConsignee(IndividualConsigneeBindingModel individualConsigneeModel)
        {
            int? createdConsigneeId = null;
            var individualConsignee = new IndividualConsignee();

            individualConsignee.FirstName = individualConsigneeModel.FirstName;
            individualConsignee.LastName = individualConsigneeModel.LastName;
            individualConsignee.TRN = individualConsigneeModel.TRN;
            individualConsignee.CustomerCode = individualConsigneeModel.CustomerCode;
            individualConsignee.Address = individualConsigneeModel.Address;

            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    createdConsigneeId = sqlConnection.ExecuteScalar<int>(
                       @"INSERT INTO IndividualConsignee (FirstName,LastName,TRN,Address,CustomerCode) 
                        OUTPUT Inserted.Id Values (@indivdualConsignee)", individualConsignee,
                      commandType: CommandType.Text, commandTimeout: 60);

                }
                catch (Exception ex)
                {
                    ex.ToString();
                    //log exceptions here

                }
            }
            return createdConsigneeId.Value;
        }

        public int CreateCompanyConsignee(CompanyConsigneeBindingModel companyConsigneeModel)
        {
            int? createdConsigneeId = null; 
            var companyCosignee = new CompanyConsignee();

            companyCosignee.CompanyName = companyConsigneeModel.CompanyName;
            companyCosignee.CompanyTRN = companyConsigneeModel.CompanyTRN;
            companyCosignee.CompanyAddress = companyConsigneeModel.CompanyAddress;
            companyCosignee.CompanyEmail = companyConsigneeModel.CompanyEmail;
            companyCosignee.CompanyPhone = companyCosignee.CompanyPhone;
            companyCosignee.CustomerCode = companyCosignee.CustomerCode;
            
            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    createdConsigneeId = sqlConnection.ExecuteScalar<int>(
                       @"INSERT INTO CompanyConsignee (CompanyName,CompanyTRN,CompanyAddress,CompanyEmail,CompanyPhoneNumber,CustomerCode) 
                        OUTPUT Inserted.Id Values (@companyConsignee)",companyCosignee, 
                      commandType: CommandType.Text, commandTimeout: 60);

                }
                catch (Exception ex)
                {
                    ex.ToString();
                    //log exceptions here

                }
            }
            return createdConsigneeId.Value;
        }

        public void CreateIndividualConsigneeRepresentative(string userId, int individualConsigneeId)
        {
            using(var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    sqlConnection.Execute("INSERT INTO CompanyRepresentative (UserId,IndividualConsigneeId) VALUES (@userId,@companyConsigneeId)",
                                        new { @userId = userId, @companyConsigneeId = individualConsigneeId }, null, 60, CommandType.Text);
                }
                catch
                {
                    //log exceptions and handling here
                }
                sqlConnection.Close();
            }    
        }

        public void CreateCompanyConsigneeRepresentative(string userId,int companyConsigneeId)
        {
            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    sqlConnection.Execute("INSERT INTO CompanyRepresentative (UserId,CompanyConsigneeId) VALUES (@userId,@companyConsigneeId)", 
                                        new { @userId = userId, @companyConsigneeId = companyConsigneeId },null,60, CommandType.Text);
                }
                catch
                {
                    //log exceptions and handling here
                }
            }
        }

        #endregion
    }

    public class WISUserInfo
    {
        public string CULOGN { get; set; }
        public string CUCODE { get; set; }
        public string CUPWORD { get; set; }
        public string CUNAME { get; set; }
        public string PCENCD { get; set; }
        public string ENQONLY { get; set; }
    }
}
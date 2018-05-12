using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Common.Models.Consignee;
using UserManagementAPI.Domain;


namespace UserManagementAPI.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private string _DBConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public string CreateToken(string serializedUserDetails)
        {
            Guid token = new Guid();
            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    token = sqlConnection.ExecuteScalar<Guid>(
                       @"INSERT INTO AuthenticationInformation (UserIdentityDetails) OUTPUT Inserted.Token Values (@userClaimsInfo)", new { userClaimsInfo = serializedUserDetails },
                      commandType: CommandType.Text, commandTimeout: 60);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "error occured creating token");
                    //ex.ToString();
                    //TODO
                }
            }
            return token.ToString();
        }

        public AuthenticationInformation GetAuthorizationInformation(string token)
        {
            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                sqlConnection.Open();
                IEnumerable<AuthenticationInformation> authToken = sqlConnection.Query<AuthenticationInformation>("SELECT * FROM AuthenticationInformation WHERE token=@Token", new { Token = token },
                    commandType: CommandType.Text, commandTimeout: 60);

                if (authToken != null && authToken.Count() > 0)
                    return authToken.First(); //since list should only ever contain one due to guid being unique
                else
                    return null;
            }
        }

        #region Consignee and Consignee Links
        
        public int? CreateCompanyConsignee(CompanyConsigneeBindingModel companyConsigneeModel)
        {
            int? createdConsigneeId = null;
            var CompanyCosignee = new CompanyConsignee();

            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    //TODO replace with stored procedures 
                    sqlConnection.Open();
                    createdConsigneeId = sqlConnection.ExecuteScalar<int>(
                       @"INSERT INTO CompanyConsignee (CompanyName,CompanyTRN,CompanyAddress,CompanyEmail,CompanyPhoneNumber)
                        OUTPUT Inserted.Id Values (@CompanyName,@CompanyTRN,@CompanyAddress,@CompanyEmail,@CompanyPhone)",
                        new
                        {
                            CompanyName = companyConsigneeModel.CompanyName,
                            CompanyTRN = companyConsigneeModel.CompanyTRN,
                            CompanyAddress = companyConsigneeModel.CompanyAddress,
                            CompanyEmail = companyConsigneeModel.CompanyEmail,
                            CompanyPhone = CompanyCosignee.CompanyPhone,
                        },
                            commandType: CommandType.Text, commandTimeout: 60);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error creating company consignee.");
                    
                    //TODO - Rethrow exception here
                    //ex.ToString();
                    //log exceptions here
                }
            }
            return createdConsigneeId;
        }

        public void CreateCompanyConsigneeRepresentative(string userId, int companyConsigneeId)
        {
            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    sqlConnection.Execute("INSERT INTO CompanyRepresentative (UserId,CompanyConsigneeId) VALUES (@userId,@companyConsigneeId)",
                                        new { @userId = userId, @companyConsigneeId = companyConsigneeId }, null, 60, CommandType.Text);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, "Error creating company consignee representative.");
                    //TODO
                    //NLOG && EXCEPTION HANDLING                    
                    //log exceptions and handling here
                }
            }
        }

        public int? CreateIndividualConsignee(IndividualConsigneeBindingModel individualConsigneeModel)
        {
            int? createdConsigneeId = null;

            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    createdConsigneeId = sqlConnection.ExecuteScalar<int>(
                       @"INSERT INTO IndividualConsignee (FirstName,LastName,TRN,Address,CustomerCode)
                        OUTPUT Inserted.Id Values (@FirstName,@LastName,@TRN,@Address,@CustomerCode)",
                        new
                        {
                            @FirstName = individualConsigneeModel.FirstName,
                            @LastName = individualConsigneeModel.LastName,
                            @TRN = individualConsigneeModel.TRN,
                            @Address = individualConsigneeModel.Address,
                            @CustomerCode = individualConsigneeModel.CustomerCode
                        },
                      commandType: CommandType.Text, commandTimeout: 60);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error creating individual consignee");
                    //TODO
                    //NLOG && EXCEPTION HANDLING
                    ex.ToString();
                    //log exceptions here
                }
            }
            return createdConsigneeId;
        }

        public void CreateIndividualConsigneeRepresentative(string userId, int individualConsigneeId)
        {
            using (var sqlConnection = new SqlConnection(_DBConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    sqlConnection.Execute("INSERT INTO IndividualRepresentative (UserId,IndividualConsigneeId) VALUES (@userId,@individualConsigneeId)",
                                        new { @userId = userId, @individualConsigneeId = individualConsigneeId }, null, 60, CommandType.Text);
                }
                catch (Exception ex)
                {
                    //TODO NLOG HERE
                    //log exceptions and handling here
                    _logger.Error(ex, "Error creating individual consignee representative.");
                }
                sqlConnection.Close();
            }
        }

        public CompanyConsignee GetCompanyConsignee(string companyTRN)
        {
            CompanyConsignee companyConsignee = null;

            try
            {
                using (var SqlConnection = new SqlConnection(_DBConnectionString))
                {
                    //select the unique company
                    companyConsignee = SqlConnection.Query<CompanyConsignee>("SELECT * FROM CompanyConsignee WHERE CompanyTRN = @Company_TRN",
                          new { @Company_TRN = companyTRN }, commandType: CommandType.Text, commandTimeout: 60).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //NLOG
                //EXCEPTION LOGGING
                var ext = ex.ToString();

            }

            return companyConsignee;
        }

        public ConsigneeListing GetConsigneeByUser(string userId)
        {
            IEnumerable<IndividualConsignee> individualConsignees = null;
            IEnumerable<CompanyConsignee> companyConsignees = null;

            try
            {
                using (var SqlConnection = new SqlConnection(_DBConnectionString))
                {
                    //TODO STORED PROCS
                    individualConsignees = SqlConnection.Query<IndividualConsignee>("SELECT FirstName,LastName,TRN,[Address],CustomerCode " +
                                                                                    "FROM IndividualConsignee Inner Join IndividualRepresentative ON " +
                                                                                    "IndividualConsignee.Id = IndividualRepresentative.IndividualConsigneeId " +
                                                                                    "WHERE UserId = @userId",
                        new { @userId = userId }, commandType: CommandType.Text, commandTimeout: 60);

                    //TODO STORED PROCS
                    companyConsignees = SqlConnection.Query<CompanyConsignee>("SELECT CompanyName,CompanyTRN,CompanyAddress,CompanyPhoneNumber,CompanyEmail,CustomerCode " +
                                                                              "FROM CompanyConsignee Inner Join CompanyRepresentative ON " +
                                                                              "CompanyConsignee.Id = CompanyRepresentative.CompanyConsigneeId " +
                                                                              "WHERE UserId = @userId",
                     new { @userId = userId }, commandType: CommandType.Text, commandTimeout: 60);
                }
            }
            catch (Exception ex)
            {
                //LOG
                //TODO
                var ext = ex.ToString();
            }

            return new ConsigneeListing { CompanyConsignees = companyConsignees, IndividualConsignees = individualConsignees };
        }

        public IndividualConsignee GetIndividualConsignee(string indivdualTRN)
        {
            IndividualConsignee individualConsignee = null;

            try
            {
                using (var SqlConnection = new SqlConnection(_DBConnectionString))
                {
                    //TODO STORED PROCS
                    individualConsignee = SqlConnection.Query<IndividualConsignee>("SELECT * FROM IndividualConsignee WHERE TRN = @IndividualTRN",
                        new { @IndividualTRN = indivdualTRN }, commandType: CommandType.Text, commandTimeout: 60).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //TODO
                _logger.Error(ex, "Error occured getting consignee data from database.");

                var ext = ex.ToString();
                //LOG
                //TODO
            }

            return individualConsignee;
        }

        #endregion Consignee and Consignee Links
    }
}
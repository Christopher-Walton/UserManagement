using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Common.Models.Consignee;
using UserManagementAPI.Identity.DataAccess;
using UserManagementAPI.Identity.Domain;
using UserManagementAPI.Identity.Infrastructure;
using UserManagementAPI.Identity.Interfaces;

namespace UserManagementAPI.Identity.Concrete
{
    public class ConsigneeManagement : IConsigneeManagement
    {
        private IDataAccess _dataAccess;

        public ConsigneeManagement(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        public int CreateConsignee(CompanyConsigneeBindingModel companyConsigneeModel)
        {
            throw new NotImplementedException();
        }

        public void CreateConsigneeUserRecord(ConsigneeRepresentativeBindingModel consigneeRepresentativeBindingModel)
        {
            throw new NotImplementedException();
        }
    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="companyConsigneeModel"></param>
    //    /// <returns>The id of newly created consignee.</returns>
    //    public int CreateConsignee(CompanyConsigneeBindingModel companyConsigneeModel)
    //    {

    //        var consignee = new Consignee();

    //        //consignee.TRN = companyConsigneeModel.TRN;
    //        //consignee.GroupEmail = companyConsigneeModel.GroupEmail;
    //        //consignee.RegisteredName = companyConsigneeModel.RegisteredName;
            
            
    //        consignee.WISAddress = companyConsigneeModel.WISAddress;
    //        consignee.WISConsignee = companyConsigneeModel.WISConsignee;
    //        consignee.CustomerCode = companyConsigneeModel.CustomerCode;

    //        //_dbContext.Consignees.Add(consignee);
    //        //_dbContext.SaveChanges();

    //        return consignee.Id;
    //    }


    //    public void CreateConsigneeUserRecord(ConsigneeRepresentativeBindingModel consigneeRepresentativeBindingModel)
    //    {
    //        //var consignee = new ConsigneeRepresentative();
    //        //consignee.ConsigneeId = consigneeRepresentativeBindingModel.ConsigneeId;
    //        //consignee.UserId = consigneeRepresentativeBindingModel.UserId;

    //        //_dbContext.ConsigneeRepresentatives.Add(consignee);
    //        //_dbContext.SaveChanges();

    //    }
    //}
}

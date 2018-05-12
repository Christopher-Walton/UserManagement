using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Common.Models
{
    public class RegistrationReturnModel<T>
    {
        public UserReturnModel UserModel { get; set; }
        public T ConsigneeModel { get; set; }
    }
}

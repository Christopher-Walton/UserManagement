using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Common.ActionResults;

namespace UserManagementAPI.BusinessLogic
{
    public interface IUtilityManager
    {
        GenericActionResult<string> SendEmailToUser(string emailAddress, string subject, string body);

        GenericActionResult<string> SendEmail(string emailAddress, string subject, string body);
    }
}

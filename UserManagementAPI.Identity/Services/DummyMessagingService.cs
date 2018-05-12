using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Identity.Services
{
    public class DummyService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            //throw exception
            throw new Exception("Dumb Exception");
        }
    }
}

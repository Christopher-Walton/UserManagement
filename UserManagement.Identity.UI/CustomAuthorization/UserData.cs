using Newtonsoft.Json;
using System.Collections.Generic;

namespace UserManagement.Identity.UI.CustomAuthorization
{
    public class UserData
    {
        public UserData()
        {
            this.WebServiceToken = null;
            this.Roles = new List<string>();
            this.UserName = null;
            this.UserId = null;
        }

        public string WebServiceToken { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public List<string> Roles { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
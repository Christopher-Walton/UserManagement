using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Common.Models
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public object Tags { get; set; }
    }

    public class RolesViewModel
    {
        public IList<Role> AvailableRoles { get; set; }
        public IList<Role> SelectedRoles { get; set; }
        public PostedRoles PostedRoles { get; set; }

        public string UserId { get; set; }
    }

    public class PostedRoles
    {
        public string UserId { get; set; }

        public string[] RoleIds { get; set; }
    }

}

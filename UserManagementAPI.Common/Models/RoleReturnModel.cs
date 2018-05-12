using System.Collections.Generic;
namespace UserManagementAPI.Common.Models
{
    public class RoleReturnModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class EditUserRolesModel
    {
        public string UserId { get; set; }

        public List<RolesEditModel> SelectedRoles {get;set;}
    }

    public class RolesEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
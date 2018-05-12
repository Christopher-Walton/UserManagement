namespace UserManagementAPI.Identity.Domain
{
    public class CompanyConsigneeRepresentative
    {
        public string UserId { get; set; }
        public int CompanyConsigneeId { get; set; }
    }

    public class IndividualConsigneeRepresentative
    {
        public string UserId { get; set; }
        public int CompanyConsigneeId { get; set; }
    }
}
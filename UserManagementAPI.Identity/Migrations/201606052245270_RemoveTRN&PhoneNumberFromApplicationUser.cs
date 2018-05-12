namespace UserManagementAPI.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTRNPhoneNumberFromApplicationUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "TRN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "TRN", c => c.String());
        }
    }
}

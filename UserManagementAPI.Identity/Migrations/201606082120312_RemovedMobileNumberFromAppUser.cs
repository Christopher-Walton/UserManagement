namespace UserManagementAPI.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedMobileNumberFromAppUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "MobileNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "MobileNumber", c => c.Long(nullable: false));
        }
    }
}

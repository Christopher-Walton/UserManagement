// <auto-generated />
namespace UserManagementAPI.Identity.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class RemoveTRNPhoneNumberFromApplicationUser : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RemoveTRNPhoneNumberFromApplicationUser));
        
        string IMigrationMetadata.Id
        {
            get { return "201606052245270_RemoveTRN&PhoneNumberFromApplicationUser"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Identity.Infrastructure;

namespace UserManagementAPI.Identity.Domain.Configurations
{
    public class ConsigneeRepresentativeConfiguration:EntityTypeConfiguration<ConsigneeRepresentative>
    {
       public ConsigneeRepresentativeConfiguration()
        {
            this.HasKey(c => new { c.UserId, c.ConsigneeId });

            this.HasRequired<ApplicationUser>(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            this.HasRequired<Consignee>(c => c.Consignee).WithMany().HasForeignKey(c => c.ConsigneeId);
        }
    }
}

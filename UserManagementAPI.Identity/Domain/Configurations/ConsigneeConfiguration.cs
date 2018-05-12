using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Identity.Domain.Configurations
{
    public class ConsigneeConfiguration: EntityTypeConfiguration<Consignee>
    {
        public ConsigneeConfiguration()
        {
            //Creates an Index on the Consignee TRN field,ensures uniqueness
            
           // Property(c => c.TRN).HasMaxLength(9).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_ConsigneeTRN") { IsUnique = true }));
        }


    }
}

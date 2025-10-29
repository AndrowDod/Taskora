using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDO_List.DAL.Data.Models;

namespace ToDO_List.DAL.Data.Configurations
{
    internal class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.DueDate)
                .IsRequired();

            builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.userId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

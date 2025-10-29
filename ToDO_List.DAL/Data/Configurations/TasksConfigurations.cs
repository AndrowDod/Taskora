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
    internal class TasksConfigurations : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                   .IsRequired()
                   .UseIdentityColumn();

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.DueDate)
                   .IsRequired();

            builder.Property(t => t.IsDone)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(t => t.projectId)
                    .IsRequired(false);

            builder.HasOne(t => t.Project)
                   .WithMany(p => p.Tasks)
                   .HasForeignKey(t => t.projectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.User)
             .WithMany()
             .HasForeignKey(t => t.userId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

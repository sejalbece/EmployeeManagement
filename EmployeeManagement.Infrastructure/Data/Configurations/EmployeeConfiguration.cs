using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
           builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.LastName)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(e => e.HireDate)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.HasIndex(e => e.UserId)
                .IsUnique();

            builder.HasOne(e=>e.User)
                .WithOne(u=>u.Employee)
                .HasForeignKey<Employee>(e=>e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

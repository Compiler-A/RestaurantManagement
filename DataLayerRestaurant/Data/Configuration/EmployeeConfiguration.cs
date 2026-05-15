using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayerRestaurant.Data.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeID);

            builder.HasIndex(e => e.Username, "UQ_Employees_UserName").IsUnique();

            builder.ToTable("tbEmployees");


            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Username).HasMaxLength(50);
            builder.Property(e => e.Password).HasMaxLength(256);

            builder.HasOne(e => e.JobRole)
                   .WithMany(j => j.Employees)
                   .HasForeignKey(e => e.JobRoleID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Employees_JobRole");
        }
    }
}

using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataLayerRestaurant.Data.Configuration
{
    public class JobRoleConfiguration : IEntityTypeConfiguration<JobRole>
    {
        public void Configure(EntityTypeBuilder<JobRole> builder)
        {
            builder.HasKey(e => e.JobRoleID);

            builder.ToTable("tbJobRoles");

            builder.HasIndex(e => e.JobName, "UQ_tbJobRoles_Name").IsUnique();

            builder.Property(e => e.JobName).HasMaxLength(50);
            builder.Property(e => e.Description).HasMaxLength(255);

        }
    }
}

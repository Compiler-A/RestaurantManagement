using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayerRestaurant.Data.Configuration
{
    public class EmployeeRefreshTokenConfiguration : IEntityTypeConfiguration<Auth>
    {
        public void Configure(EntityTypeBuilder<Auth> builder)
        {
            builder.ToTable("tbEmployeeRefreshTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RefreshTokenHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.RefreshTokenExpiresAt)
                .IsRequired();

            builder.Property(x => x.RefreshTokenRevokedAt)
                .IsRequired(false);

            builder.HasOne(x => x.Employee)
                .WithMany(e => e.RefreshTokens)
                .HasForeignKey(x => x.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EmployeeRefreshTokens_Employee");
        }
    }
}
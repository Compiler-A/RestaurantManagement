using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayerRestaurant.Data.Configuration
{
    public class StatusMenuConfiguration : IEntityTypeConfiguration<StatusMenu>
    {
        public void Configure(EntityTypeBuilder<StatusMenu> builder)
        {
            builder.HasKey(e => e.StatusMenuID);

            builder.ToTable("tbStatusMenus");

            builder.HasIndex(e => e.StatusMenuName, "UQ_tbStatusMenus_Name").IsUnique();

            builder.Property(e => e.StatusMenuName).HasMaxLength(50);
            builder.Property(e => e.Description).HasMaxLength(255);

        }
    }
}

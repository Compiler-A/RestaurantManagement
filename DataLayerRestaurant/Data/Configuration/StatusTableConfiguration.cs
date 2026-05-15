
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayerRestaurant.Data.Configuration
{
    internal class StatusTableConfiguration : IEntityTypeConfiguration<StatusTable>
    {
        public void Configure(EntityTypeBuilder<StatusTable> builder)
        {
            builder.HasKey(e => e.StatusTableID);
            builder.ToTable("tbStatusTables");

            builder.HasIndex(e => e.StatusTableName, "UQ_tbStatusTables_Name").IsUnique();

            builder.Property(e => e.StatusTableName).HasMaxLength(50);

        }
    }
}

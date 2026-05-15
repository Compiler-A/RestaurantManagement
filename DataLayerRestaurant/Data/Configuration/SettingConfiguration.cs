using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Data.Configuration
{
    internal class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {

            builder.HasKey(e => e.SettingID);

            builder.ToTable("tbSettings");

            builder.HasIndex(e => e.Name, "UQ_tbSettings_Name").IsUnique();

            builder.Property(e => e.Name).HasMaxLength(50);
            builder.Property(e=> e.Value).HasColumnType("decimal(10,2)");

        }
    }
}

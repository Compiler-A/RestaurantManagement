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
    public class TypeItemConfiguration : IEntityTypeConfiguration<TypeItem>
    {
        public void Configure(EntityTypeBuilder<TypeItem> builder)
        {
            builder.HasKey(e => e.TypeItemID);
            builder.ToTable("tbTypeItems");

            builder.HasIndex(e => e.TypeName, "UQ_tbTypeItems_Name").IsUnique();

            builder.Property(e => e.TypeName).HasMaxLength(50);

            builder.Property(e => e.TypeDescription).HasMaxLength(255);
        }
    }
}

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
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        { 
            builder.HasKey(e => e.ItemID);

            builder.ToTable("tbMenuItems");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_MenuItems_Price", "[Price] > 0");
            });

            builder.HasIndex(e => e.ItemName, "UQ_tbMenuItems_Name").IsUnique();

            builder.Property(e => e.ItemName).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(255);
            builder.Property(e => e.Price).HasColumnType("decimal(10,2)");


            builder.HasOne(e => e.TypeItem)
                   .WithMany(t => t.MenuItems)
                   .HasForeignKey(e => e.TypeItemID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_MenuItems_TypeItem");

            builder.HasOne(e => e.StatusMenu)
                   .WithMany(s => s.MenuItems)
                   .HasForeignKey(e => e.StatusMenuID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_MenuItems_StatusMenu");
        }
    }
}

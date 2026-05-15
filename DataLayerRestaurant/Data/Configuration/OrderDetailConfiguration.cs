using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayerRestaurant.Data.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("tbOrderDetails");

            builder.HasKey(x => x.OrderDetailID);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.SubTotal)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_tbOrderDetails_Quantity",
                    "[Quantity] >= 1"
                );

                t.HasCheckConstraint(
                    "CK_tbOrderDetails_SubTotal",
                    "[Subtotal] >= 0"
                );
            });


            builder.HasOne(x => x.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(x => x.OrderID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderDetails_Order");

            builder.HasOne(x => x.Item)
                .WithMany(i => i.OrderDetails)
                .HasForeignKey(x => x.ItemID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderDetails_MenuItem");
        }
    }
}
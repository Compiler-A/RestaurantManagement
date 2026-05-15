using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayerRestaurant.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderID);

            builder.ToTable("tbOrders");

            builder.Property(x => x.OrderDate)
                .HasDefaultValueSql("getdate()");

            builder.Property(x => x.TotalAmount)
                .HasColumnType("decimal(10,2)");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_tbOrders_TotalAmount",
                    "[TotalAmount] >= 0"
                );
            });


            builder.HasOne(x => x.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(x => x.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Orders_Employee");

            builder.HasOne(x => x.Table)
                .WithMany(t => t.Orders)
                .HasForeignKey(x => x.TableID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Orders_Table");

            builder.HasOne(x => x.StatusOrder)
                .WithMany(s => s.Orders)
                .HasForeignKey(x => x.StatusOrderID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Orders_StatusOrder");
        }
    }
}
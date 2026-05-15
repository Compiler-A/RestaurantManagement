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
    internal class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.HasKey(e => e.TableID);
            builder.ToTable("tbTables");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_tbTables_Seats", "[Seats]=(6) OR [Seats]=(4) OR [Seats]=(2)");
            });

            builder.HasIndex(e => e.TableNumber, "UQ_tbTables_TableNumber").IsUnique();
            
            builder.Property(e => e.TableNumber).HasMaxLength(50);
            builder.Property(e => e.Seats);

            builder.HasOne(e => e.StatusTable)
                .WithMany(s => s.Tables)
                .HasForeignKey(e => e.StatusTableID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Tables_StatusTable");
        }
    }
}

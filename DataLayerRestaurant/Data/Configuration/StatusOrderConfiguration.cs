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
    public class StatusOrderConfiguration : IEntityTypeConfiguration<StatusOrder>
    {
        public void Configure(EntityTypeBuilder<StatusOrder> builder)
        {
            builder.HasKey(e => e.StatusOrderID);
            builder.ToTable("tbStatusOrders");

            builder.HasIndex(e => e.StatusOrderName, "UQ_tbStatusOrders_Name").IsUnique();

            builder.Property(e => e.StatusOrderName).HasMaxLength(50);

        }
    }
}

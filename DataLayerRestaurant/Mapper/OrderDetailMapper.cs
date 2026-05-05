using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class OrderDetailMapper
    {
        public static OrderDetail ReaderToEntity(SqlDataReader reader)
        {
            return new OrderDetail
            {
                ID = reader.GetInt32(reader.GetOrdinal("OrderDetailID")),
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                SubTotal = reader.GetDecimal(reader.GetOrdinal("SubTotal"))
            };
        }

        public static OrderDetail ReaderToEntityResult(SqlDataReader reader)
        {
            return new OrderDetail
            {
                ID = reader.GetInt32(reader.GetOrdinal("OrderDetailID")),
                ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Order = new Order
                {
                    ID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                },
                Item = new MenuItem
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                    Name = reader.GetString(reader.GetOrdinal("ItemName")),
                },
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                SubTotal = reader.GetDecimal(reader.GetOrdinal("SubTotal"))
            };
        }   
    }
}

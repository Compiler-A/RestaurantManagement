using DomainLayer.Entities;
using Microsoft.Data.SqlClient;


namespace DataLayerRestaurant.Mapper
{
    public class OrderMapper
    {
        public static Order ReaderToEntity(SqlDataReader reader)
        {
            return new Order
            {
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                TableID = reader.GetInt32(reader.GetOrdinal("TableID")),
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                StatusOrderID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
            };
        }

        public static Order ReaderToEntityResult(SqlDataReader reader)
        {
            return new Order
            {
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                TableID = reader.GetInt32(reader.GetOrdinal("TableID")),
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                StatusOrderID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                Employee = new Employee
                {
                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                    Username = reader.GetString(reader.GetOrdinal("username"))
                },
                Table = new Table
                {
                    TableID = reader.GetInt32(reader.GetOrdinal("TableID")),
                    TableNumber = reader.GetString(reader.GetOrdinal("TableNumber"))
                },
                StatusOrder = new StatusOrder
                {
                    StatusOrderID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                    StatusOrderName = reader.GetString(reader.GetOrdinal("StatusOrderName"))
                }
                
            };
        }
    }
}

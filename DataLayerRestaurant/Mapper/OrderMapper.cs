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
                ID = reader.GetInt32(reader.GetOrdinal("OrderID")),
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
                ID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                TableID = reader.GetInt32(reader.GetOrdinal("TableID")),
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                StatusOrderID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                employees = new Employee
                {
                    ID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                    UserName = reader.GetString(reader.GetOrdinal("username"))
                },
                tables = new Table
                {
                    ID = reader.GetInt32(reader.GetOrdinal("TableID")),
                    Name = reader.GetString(reader.GetOrdinal("TableNumber"))
                },
                statusOrders = new StatusOrder
                {
                    ID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                    Name = reader.GetString(reader.GetOrdinal("StatusOrderName"))
                }
                
            };
        }
    }
}

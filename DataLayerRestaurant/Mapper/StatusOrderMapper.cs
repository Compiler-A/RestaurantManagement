using DomainLayer.Entities;
using Microsoft.Data.SqlClient;


namespace DataLayerRestaurant.Mapper
{
    public class StatusOrderMapper
    {
        public static StatusOrder ReaderToEntity(SqlDataReader reader)
        {
            return new StatusOrder
            {
                StatusOrderID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                StatusOrderName = reader.GetString(reader.GetOrdinal("StatusOrderName"))
            };
        }
    }
}

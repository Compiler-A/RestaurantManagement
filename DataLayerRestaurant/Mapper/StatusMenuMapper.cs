using DomainLayer.Entities;
using Microsoft.Data.SqlClient;

namespace DataLayerRestaurant.Mapper
{
    public class StatusMenuMapper
    {
        public static StatusMenu ReaderToEntity(SqlDataReader reader)
        {
            return new StatusMenu
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                Name = reader.GetString(reader.GetOrdinal("StatusMenuName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }
}

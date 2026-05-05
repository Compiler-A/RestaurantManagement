using DomainLayer.Entities;
using Microsoft.Data.SqlClient;


namespace DataLayerRestaurant.Mapper
{
    public class TypeItemMapper
    {
        public static TypeItem ReaderToEntity(SqlDataReader reader)
        {
            return new TypeItem
            {
                ID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                Name = reader.GetString(reader.GetOrdinal("TypeName")),
                Description = reader.IsDBNull(reader.GetOrdinal("TypeDescription"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("TypeDescription"))
            };
        }
    }
}

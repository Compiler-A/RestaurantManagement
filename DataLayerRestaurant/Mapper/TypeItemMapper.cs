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
                TypeItemID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                TypeDescription = reader.IsDBNull(reader.GetOrdinal("TypeDescription"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("TypeDescription"))
            };
        }
    }
}

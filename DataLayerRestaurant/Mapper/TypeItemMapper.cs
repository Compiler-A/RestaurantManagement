using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class TypeItemMapper
    {
        public static TypeItem ReaderToEntity(SqlDataReader reader)
        {
            return new TypeItem
            {
                ID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                Name = reader.GetString(reader.GetOrdinal("TypeItemName")),
                Description = reader.IsDBNull(reader.GetOrdinal("TypeDescription"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("TypeDescription"))
            };
        }
    }
}

using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

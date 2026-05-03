using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class StatusOrderMapper
    {
        public static StatusOrder ReaderToEntity(SqlDataReader reader)
        {
            return new StatusOrder
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                Name = reader.GetString(reader.GetOrdinal("StatusOrderName"))
            };
        }
    }
}

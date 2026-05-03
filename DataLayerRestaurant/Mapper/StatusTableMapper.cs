using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class StatusTableMapper
    {
        public static StatusTable ReaderToEntity(SqlDataReader reader)
        {
            return new StatusTable
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusTableID")),
                Name = reader.GetString(reader.GetOrdinal("StatusTableName")),
            };
        }
    }
}

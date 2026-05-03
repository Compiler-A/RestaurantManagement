using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class SettingMapper
    {
        public static Setting ReaderToEntity(SqlDataReader reader)
        {
            return new Setting
            {
                ID = reader.GetInt32(reader.GetOrdinal("SettingID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Value = reader.GetDecimal(reader.GetOrdinal("Value"))
            };
        }
    }
}

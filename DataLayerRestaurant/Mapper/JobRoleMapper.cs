using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class JobRoleMapper
    {
        public static JobRole ReaderToEntity(SqlDataReader reader)
        {
            return new JobRole
            {
                ID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                Name = reader.GetString(reader.GetOrdinal("JobName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }
}

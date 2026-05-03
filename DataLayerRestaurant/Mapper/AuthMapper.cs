using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class AuthMapper
    {

        public static Auth ReaderToEntity(SqlDataReader reader)
        {
            return new Auth
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                RefreshTokenHash = reader.GetString(reader.GetOrdinal("RefreshTokenHash")),
                RefreshTokenExpiresAt = reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt")),
                RefreshTokenRevokedAt = reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt"))
                    ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenRevokedAt"))
            };
        }

    }
}

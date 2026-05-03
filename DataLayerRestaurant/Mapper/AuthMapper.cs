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
                Employees = new Employee
                {
                    ID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                    UserName = reader.GetString(reader.GetOrdinal("Username")),
                    JobRoles = new JobRole
                    {
                        Name = reader.GetString(reader.GetOrdinal("JobName"))
                    }
                },

                RefreshTokenHash = reader.GetString(reader.GetOrdinal("RefreshTokenHash")),
                RefreshTokenExpiresAt = reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt")),
                RefreshTokenRevokedAt = reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt"))
                    ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenRevokedAt"))
            };
        }

        public static Auth ReaderToEntityLogin(SqlDataReader reader)
        {
            return new Auth
            {
                Employees = new Employee
                {
                    ID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                    PasswordHashed = reader.GetString(reader.GetOrdinal("Password")),
                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                    JobRoles = new JobRole
                    {
                        Name = reader.GetString(reader.GetOrdinal("JobName"))
                    }
                }
            };
        }
    }
}

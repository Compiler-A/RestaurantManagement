using DomainLayer.Entities;
using Microsoft.Data.SqlClient;


namespace DataLayerRestaurant.Mapper
{
    public class AuthMapper
    {

        public static Auth ReaderToEntity(SqlDataReader reader)
        {
            return new Auth
            {
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                Employee = new Employee
                {
                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    JobRole = new JobRole
                    {
                        JobName = reader.GetString(reader.GetOrdinal("JobName"))
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
                Employee = new Employee
                {
                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    Username = reader.GetString(reader.GetOrdinal("UserName")),
                    JobRole = new JobRole
                    {
                        JobName = reader.GetString(reader.GetOrdinal("JobName"))
                    }
                }
            };
        }
    }
}

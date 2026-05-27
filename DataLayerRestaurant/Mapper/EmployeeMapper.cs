using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System.Data;


namespace DataLayerRestaurant.Mapper
{
    public class EmployeeMapper
    {
        public static Employee ReaderToEntity(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                JobRoleID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                Username = reader.GetString(reader.GetOrdinal("UserName")),
                Password = reader.GetString(reader.GetOrdinal("Password"))
            };
        }

        public static Employee ReaderToEntityResult(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                JobRoleID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                Username = reader.GetString(reader.GetOrdinal("UserName")),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                JobRole = new JobRole
                {
                    JobRoleID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                    JobName = reader.GetString(reader.GetOrdinal("JobName"))
                },
                PersonID = reader.GetInt32(reader.GetOrdinal("PersonID")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                Gender = reader.GetString(reader.GetOrdinal("Gender"))[0],
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                ProfileImage = reader.IsDBNull(reader.GetOrdinal("ProfileImage")) ? null : reader.GetString(reader.GetOrdinal("ProfileImage"))
            };
        }
    }
}

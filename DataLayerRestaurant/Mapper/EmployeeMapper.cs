using DomainLayer.Entities;
using Microsoft.Data.SqlClient;


namespace DataLayerRestaurant.Mapper
{
    public class EmployeeMapper
    {
        public static Employee ReaderToEntity(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
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
                Name = reader.GetString(reader.GetOrdinal("Name")),
                JobRoleID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                Username = reader.GetString(reader.GetOrdinal("UserName")),
                JobRole = new JobRole
                {
                    JobRoleID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                    JobName = reader.GetString(reader.GetOrdinal("JobName"))
                }
            };
        }
    }
}

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
                ID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                JobID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                PasswordHashed = reader.GetString(reader.GetOrdinal("Password"))
            };
        }

        public static Employee ReaderToEntityResult(SqlDataReader reader)
        {
            return new Employee
            {
                ID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                JobID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                JobRoles = new JobRole
                {
                    ID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                    Name = reader.GetString(reader.GetOrdinal("JobName"))
                }
            };
        }
    }
}

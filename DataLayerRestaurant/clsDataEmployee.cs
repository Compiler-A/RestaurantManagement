using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class DTOEmployees 
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int JobID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DTOJobRoles? JobRoles { get; set; }

        public DTOEmployees()
        {
            ID = -1;
            Name = string.Empty;
            JobID = -1;
            UserName = string.Empty;
            Password = string.Empty;
        }
        public DTOEmployees(int id, string name, int jobID, string userName, string password)
        {
            ID = id;
            Name = name;
            JobID = jobID;
            UserName = userName;
            Password = password;
        }
    }

    public interface IWritableEmployeeData
    {
        Task<int> Add(DTOEmployees employee);
        Task<bool> Update(DTOEmployees employee);
        Task<bool> Delete(int ID);
    }

    public interface IReadableEmployeeData
    {
        Task<bool> LoginEmployee(string UserName, string Password);
        Task<List<DTOEmployees>> GetAllEmployees(int page);
        Task<DTOEmployees?> GetEmployee(int ID);
        Task<DTOEmployees?> GetEmployee(string UserName);
    }

    public interface IDataEmployee : IReadableEmployeeData, IWritableEmployeeData { }

    public class clsGlobal
    {
        public static string HashString(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return  Convert.ToBase64String(hashBytes);
            }
        }
    }

    public class clsDataEmployee :IDataEmployee
    {

        private DTOEmployees MapToDTOEmployee(SqlDataReader reader)
        {
            DTOEmployees employee = new DTOEmployees
            {
                ID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                JobID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                Password = reader.GetString(reader.GetOrdinal("Password"))
            };
            return employee;
        }
        public async Task<DTOEmployees?> GetEmployee(string UserName)
        {
            DTOEmployees? employee = null;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("[Employees].[SP_GetEmployeeByUserName]", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@UserName", UserName);
                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            employee = MapToDTOEmployee(reader);
                        }
                    }
                }
            }
            return employee;
        }

        public async Task<bool> LoginEmployee(string UserName, string Password)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_LoginEmployee", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@UserName", UserName);
                    Command.Parameters.AddWithValue("@Password", clsGlobal.HashString(Password));
                    SqlParameter output = new SqlParameter("@Find", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(output);

                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    if (Convert.ToInt32(output.Value) == 0 || output.Value == DBNull.Value)
                        return false;
                }
            }
            return true;
        }

        public async Task<List<DTOEmployees>> GetAllEmployees(int page)
        {
            List<DTOEmployees> employees = new List<DTOEmployees>();

            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_GetAllEmployees", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(MapToDTOEmployee(reader));
                        }
                    }
                }
            }

            return employees;
        }

        public async Task<DTOEmployees?> GetEmployee(int ID)
        {
            DTOEmployees? employee = null;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("[Employees].[SP_GetEmployeeByID]", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);
                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            employee = MapToDTOEmployee(reader);
                        }
                    }
                }
            }
            return employee;
        }

        public async Task<int> Add(DTOEmployees employee)
        {
            int NewID = -1;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_AddEmployee", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", employee.Name);
                    Command.Parameters.AddWithValue("@JobID", employee.JobID);
                    Command.Parameters.AddWithValue("@UserName", employee.UserName);
                    Command.Parameters.AddWithValue("@Password", employee.Password);
                    SqlParameter output = new SqlParameter("@NewID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(output);
                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    if (output.Value == null || output.Value == DBNull.Value)
                        return -1;
                    NewID = (int)output.Value;
                }
            }

            return NewID;
        }

        public async Task<bool> Update(DTOEmployees employee)
        {
            bool Updated = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_UpdateEmployee", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", employee.ID);
                    Command.Parameters.AddWithValue("@Name", employee.Name);
                    Command.Parameters.AddWithValue("@JobID", employee.JobID);
                    Command.Parameters.AddWithValue("@UserName", employee.UserName);
                    Command.Parameters.AddWithValue("@Password", employee.Password);
                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    Updated = rowsAffected > 0;
                }
            }
            return Updated;
        }

        public async Task<bool> Delete(int ID)
        {
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_DeleteEmployee", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    Deleted = rowsAffected > 0;
                }
            }
            return Deleted;
        }
    }
}

using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace DataLayerRestaurant
{

    public class DTOEmployeesCRequest
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "JobID must be greater than 0")]
        public int JobID { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public DTOEmployeesCRequest(string name, int jobID, string userName, string password)
        {
            Name = name;
            JobID = jobID;
            UserName = userName;
            Password = password;
        }
    }

    public class DTOEmployeesURequest : DTOEmployeesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0")]
        public int ID { get; set; }

        public DTOEmployeesURequest(int ID, string Name, int JobID, string userName, string password) 
            : base(Name, JobID, userName, password)
        {
            this.ID = ID;
        }

    }

    public class DTOEmployeesLoginRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public DTOEmployeesLoginRequest(string UserName , string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
    }

    public class DTOEmployeesChangedPassword
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0")]
        public int ID { get; set; }

        [Required(ErrorMessage = "CurrentPassword is required")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "NewPassword is required")]
        public string NewPassword { get; set; } = string.Empty;

        public DTOEmployeesChangedPassword(int ID, string CurrentPassword, string NewPassword)
        {
            this.ID = ID;
            this.CurrentPassword = CurrentPassword;
            this.NewPassword = NewPassword;
        }
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

   
    public class clsHashing
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

    public class clsCompositionDEmployees : clsHashing ,ICompositionDataBase<DTOEmployees>
    {
        public DTOEmployees GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOEmployees
            {
                ID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                JobID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                Password = reader.GetString(reader.GetOrdinal("Password"))
            };
        }
    }


    public class clsEmployeesReader :clsCompositionDEmployees ,IReadableDEmployees
    {
        private readonly clsMySettings _Setting;
        public clsEmployeesReader(IOptions<clsMySettings> settings)
        {
            _Setting = settings.Value;
        }

        public async Task<DTOEmployees?> GetDataAsync(string UserName)
        {
            DTOEmployees? employee = null;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
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
                            employee = GetDataFromDataBase(reader);
                        }
                    }
                }
            }
            return employee;
        }

        public async Task<DTOEmployees?> GetDataLoginAsync(DTOEmployeesLoginRequest Request)
        {
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_LoginEmployee", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@UserName", Request.UserName);
                    Command.Parameters.AddWithValue("@Password", clsHashing.HashString(Request.Password));


                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return  GetDataFromDataBase(reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<DTOEmployees>> GetAllDataAsync(int page)
        {
            List<DTOEmployees> employees = new List<DTOEmployees>();

            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_GetAllEmployees", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", _Setting.RowsPerPage);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(GetDataFromDataBase(reader));
                        }
                    }
                }
            }

            return employees;
        }

        public async Task<DTOEmployees?> GetDataAsync(int ID)
        {
            DTOEmployees? employee = null;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
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
                            employee = GetDataFromDataBase(reader);
                        }
                    }
                }
            }
            return employee;
        }
    }


    public class clsEmployeesWriter: clsCompositionDEmployees , IWritableDEmployees
    {
        private readonly clsMySettings _Setting;
        public clsEmployeesWriter(IOptions<clsMySettings> settings)
        {
            _Setting = settings.Value;
        }

        public async Task<bool> ChangedDataPasswordAsync(DTOEmployeesChangedPassword clsChanged)
        {
            bool Changed = false;

            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_ChangedPassword", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", clsChanged.ID);
                    Command.Parameters.AddWithValue("@NewPassword", clsHashing.HashString(clsChanged.NewPassword));
                    Command.Parameters.AddWithValue("@CurrentPassword", clsHashing.HashString(clsChanged.CurrentPassword));
                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    Changed = rowsAffected > 0;
                }
            }
            return Changed;
        }

        public async Task<DTOEmployees?> CreateDataAsync(DTOEmployeesCRequest employee)
        {
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
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
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return GetDataFromDataBase(reader);
                        }
                    }
                }
            }

            return null;
        }

        public async Task<DTOEmployees?> UpdateDataAsync(DTOEmployeesURequest employee)
        {
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
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
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return GetDataFromDataBase(reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
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

    public class clsDataEmployees :IDataEmployees
    {
        private IReadableDEmployees _IRead;
        private IWritableDEmployees _IWrite;

        public clsDataEmployees(IReadableDEmployees Read, IWritableDEmployees Write)
        {
            _IRead = Read;
            _IWrite = Write;
        }


        public async Task<List<DTOEmployees>> GetAllEmployeesAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }
        public async Task<DTOEmployees?> GetEmployeeAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<DTOEmployees?> GetEmployeeAsync(string Name)
        {
            return await _IRead.GetDataAsync(Name);
        }

        public async Task<DTOEmployees?> GetLoginEmployeeAsync(DTOEmployeesLoginRequest Request)
        {
            return await _IRead.GetDataLoginAsync(Request);

        }

        public async Task<bool> ChangedPasswordEmployeeAsync(DTOEmployeesChangedPassword Request)
        {
            return await _IWrite.ChangedDataPasswordAsync(Request);
        }
        
        public async Task<DTOEmployees?> AddEmployeeAsync(DTOEmployeesCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }
        public async Task<DTOEmployees?> UpdateEmployeeAsync(DTOEmployeesURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteEmployeeAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }
}

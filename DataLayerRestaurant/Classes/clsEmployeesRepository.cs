using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.Employees;
using DataLayerRestaurant.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace DataLayerRestaurant.Classes
{

    public class clsEmployeesRepositoryComposition : ICompositionDataBase<DTOEmployees>
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


    public class clsEmployeesRepositoryReader :clsEmployeesRepositoryComposition ,IEmployeesRepositoryReader
    {
        private readonly clsMySettings _Setting;
        public clsEmployeesRepositoryReader(IOptions<clsMySettings> settings)
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


    public class clsEmployeesRepositoryWriter: clsEmployeesRepositoryComposition , IEmployeesRepositoryWriter
    {
        private readonly clsMySettings _Setting;
        public clsEmployeesRepositoryWriter(IOptions<clsMySettings> settings)
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
                    Command.Parameters.AddWithValue("@NewPassword", (clsChanged.NewPassword));
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

    public class clsEmployeesRepository : IEmployeesRepository
    {
        private IEmployeesRepositoryReader _IRead;
        private IEmployeesRepositoryWriter _IWrite;

        public clsEmployeesRepository(IEmployeesRepositoryReader Read, IEmployeesRepositoryWriter Write)
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

using ContractsLayerRestaurant.DTORequest.Employees;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.Reflection;

namespace DataLayerRestaurant.Classes.SQL
{

    public class EmployeesRepositoryReader : IEmployeesRepositoryReader
    {
        private readonly clsMySettings _Setting;
        public EmployeesRepositoryReader(IOptions<clsMySettings> settings)
        {
            _Setting = settings.Value;
        }
        public async Task<List<Employee>> GetAllDataAsync(List<int> Ids)
        {
            List<Employee> result = new List<Employee>();
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_GetAllEmployeesByIds", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;

                    var param = new SqlParameter("@Ids", SqlDbType.Structured)
                    {
                        TypeName = "dbo.IntList",
                        Value = CreateSqlRecords.CreateSqlRecord(Ids)
                    };
                    Command.Parameters.Add(param);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(EmployeeMapper.ReaderToEntityResult(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<Employee?> GetDataAsync(string UserName)
        {
            Employee? employee = null;
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
                            employee = EmployeeMapper.ReaderToEntityResult(reader);
                        }
                    }
                }
            }
            return employee;
        }


        public async Task<List<Employee>> GetAllDataAsync(int page)
        {
            List<Employee> employees = new List<Employee>();

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
                            employees.Add(EmployeeMapper.ReaderToEntityResult(reader));
                        }
                    }
                }
            }
            return employees;
        }

        public async Task<Employee?> GetDataAsync(int ID)
        {
            Employee? employee = null;
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
                            employee = EmployeeMapper.ReaderToEntityResult(reader);
                        }
                    }
                }
            }
            return employee;
        }
    }


    public class EmployeesRepositoryWriter : IEmployeesRepositoryWriter
    {
        private readonly clsMySettings _Setting;
        public EmployeesRepositoryWriter(IOptions<clsMySettings> settings)
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

        public async Task<Employee?> CreateDataAsync(DTOEmployeesCRequest employee)
        {
            Employee? New = null;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_AddEmployee", Connection))
                { 
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    Command.Parameters.AddWithValue("@LastName", employee.LastName);
                    Command.Parameters.AddWithValue("@Gender", employee.Gender);
                    Command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    Command.Parameters.AddWithValue("@ProfileImage", (object?)employee.ProfileImage ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@JobID", employee.JobID);
                    Command.Parameters.AddWithValue("@UserName", employee.UserName);
                    Command.Parameters.AddWithValue("@Password", employee.Password);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            New = EmployeeMapper.ReaderToEntityResult(reader);
                        }
                    }
                }
            }
            return New;
        }

        public async Task<Employee?> UpdateDataAsync(DTOEmployeesURequest employee)
        {
            Employee? Update = null;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Employees.SP_UpdateEmployee", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", employee.ID);
                    Command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    Command.Parameters.AddWithValue("@LastName", employee.LastName);
                    Command.Parameters.AddWithValue("@Gender", employee.Gender);
                    Command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    Command.Parameters.AddWithValue("@ProfileImage", (object?)employee.ProfileImage ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@JobID", employee.JobID);
                    Command.Parameters.AddWithValue("@UserName", employee.UserName);
                    Command.Parameters.AddWithValue("@Password", employee.Password);
                    Command.Parameters.AddWithValue("@PersonID", employee.PersonID);
                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Update = EmployeeMapper.ReaderToEntityResult(reader);
                        }
                    }
                }
            }

            return Update;
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
}

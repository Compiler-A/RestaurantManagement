using ContractsLayerRestaurant.DTORequest.Auth;
using DataLayerRestaurant.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DomainLayer.Entities;
using DataLayerRestaurant.Mapper;


namespace DataLayerRestaurant.Classes
{


    public class clsLoginRepositoryReader : ILoginRepositoryReader
    {
        protected readonly clsMySettings _Setting;
        public clsLoginRepositoryReader(IOptions<clsMySettings> Setting) 
        {
            _Setting = Setting.Value;
        }

        public async Task<Auth?> LoginAsync(string UserName)
        {
            Auth? Login = null;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("EmployeeRefreshTokens.Login", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@UserName", UserName);
                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Login = AuthMapper.ReaderToEntityLogin(reader);
                        }
                    }
                }
            }
            return Login;
        }

        public async Task<Auth?> GetDataAsync(string UserName)
        {
            Auth? Login = null;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("EmployeeRefreshTokens.SP_GetToken", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@UserName", UserName);
                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Login = AuthMapper.ReaderToEntity(reader);
                        }
                    }
                }
            }
            return Login;
        }
    }

    public class clsLoginRepositoryWriter : ILoginRepositoryWriter
    {
        private readonly clsMySettings _Setting;
        public clsLoginRepositoryWriter(IOptions<clsMySettings> settings)
        {
            _Setting = settings.Value;
        }

        public async Task<bool> CreateDataAsync(DTOAuthCURequest Request)
        {
            bool Create = false;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("EmployeeRefreshTokens.SP_CreateToken", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@EmployeeID", Request.EmployeeID);
                    Command.Parameters.AddWithValue("@RefreshToken", Request.RefreshTokenHash);
                    Command.Parameters.AddWithValue("@ExpiresAt", Request.RefreshTokenExpiresAt);

                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    Create = rowsAffected > 0;
                }
            }
            return Create;
        }
        public async Task<bool> LogoutDataAsync(int EmployeeID)
        {
            bool Update = false;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("EmployeeRefreshTokens.SP_LogoutToken", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    Update = rowsAffected > 0;
                }
            }
            return Update;
        }
        public async Task<bool> UpdateDataAsync(DTOAuthCURequest Request)
        {
            bool Update = false;
            using (SqlConnection Connection = new SqlConnection(_Setting.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("EmployeeRefreshTokens.SP_UpdateToken", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@EmployeeID", Request.EmployeeID);
                    Command.Parameters.AddWithValue("@RefreshToken", Request.RefreshTokenHash);
                    Command.Parameters.AddWithValue("@ExpiresAt", Request.RefreshTokenExpiresAt);

                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    Update = rowsAffected > 0;
                }
            }
            return Update;
        }
    }

    public class clsLoginRepository : ILoginRepository
    {
        private readonly ILoginRepositoryReader _reader;
        private readonly ILoginRepositoryWriter _writer;
        public clsLoginRepository(ILoginRepositoryReader reader, ILoginRepositoryWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public async Task<Auth?> LoginAsync(string UserName)
        {
            return await _reader.LoginAsync(UserName);
        }

        public async Task<bool> CreateDataAsync(DTOAuthCURequest Request)
        {
            return await _writer.CreateDataAsync(Request);
        }
        public async Task<bool> LogoutDataAsync(int EmployeeID)
        {
            return await _writer.LogoutDataAsync(EmployeeID);
        }
        public async Task<bool> UpdateDataAsync(DTOAuthCURequest Request)
        {
            return await _writer.UpdateDataAsync(Request);
        }
        public async Task<Auth?> GetDataAsync(string UserName)
        {
            return await _reader.GetDataAsync(UserName);
        }
    }
}

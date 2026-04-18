using ContractsLayerRestaurant.DTOs.Employees;
using ContractsLayerRestaurant.DTOs.Auth;
using DataLayerRestaurant.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;


namespace DataLayerRestaurant.Classes
{


    public class clsLoginRepositoryComposition : ICompositionDataBase<DTOLogin>
    {
        protected readonly clsMySettings _Setting;
        public clsLoginRepositoryComposition(IOptions<clsMySettings> settings)
        {
            _Setting = settings.Value;
        }

        public DTOLogin GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOLogin
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                RefreshTokenHash = reader.GetString(reader.GetOrdinal("RefreshTokenHash")),
                RefreshTokenExpiresAt = reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt")),
                RefreshTokenRevokedAt = reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt")) 
                    ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenRevokedAt"))
            };
        }
    }

    public class clsLoginRepositoryReader : clsLoginRepositoryComposition,ILoginRepositoryReader
    {
        public clsLoginRepositoryReader(IOptions<clsMySettings> settings) : base(settings)
        { }
        public async Task<DTOLogin?> GetDataAsync(string UserName)
        {
            DTOLogin? Login = null;
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
                            Login = GetDataFromDataBase(reader);
                        }
                    }
                }
            }
            return Login;
        }
    }

    public class clsLoginRepositoryWriter : clsLoginRepositoryComposition, ILoginRepositoryWriter
    {
        public clsLoginRepositoryWriter(IOptions<clsMySettings> settings) : base(settings)
        { }

        public async Task<bool> CreateDataAsync(DTOLoginCURequest Request)
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
        public async Task<bool> UpdateDataAsync(DTOLoginCURequest Request)
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
        public async Task<bool> CreateDataAsync(DTOLoginCURequest Request)
        {
            return await _writer.CreateDataAsync(Request);
        }
        public async Task<bool> LogoutDataAsync(int EmployeeID)
        {
            return await _writer.LogoutDataAsync(EmployeeID);
        }
        public async Task<bool> UpdateDataAsync(DTOLoginCURequest Request)
        {
            return await _writer.UpdateDataAsync(Request);
        }
        public async Task<DTOLogin?> GetDataAsync(string UserName)
        {
            return await _reader.GetDataAsync(UserName);
        }
    }
}

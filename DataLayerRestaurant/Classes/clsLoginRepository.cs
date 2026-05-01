using ContractsLayerRestaurant.DTORequest.Auth;
using DataLayerRestaurant.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Classes
{


    public class clsLoginRepositoryComposition : ICompositionDataBase<Auth>
    {
        protected readonly clsMySettings _Setting;
        public clsLoginRepositoryComposition(IOptions<clsMySettings> settings)
        {
            _Setting = settings.Value;
        }

        public Auth GetDataFromDataBase(SqlDataReader reader)
        {
            return new Auth
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


    public class clsEmployeeBatchLoaderByAuth : IRepositoryBatchsLoader<Auth>
    {
        private readonly IEmployeesRepositoryReader _service;
        public clsEmployeeBatchLoaderByAuth(IEmployeesRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<Auth> auths)
        {
            var employeeIDs = auths.Select(e => e.EmployeeID).Distinct().ToList();
            if (!employeeIDs.Any())
                return;
            var roles = await _service.GetAllDataAsync(employeeIDs);
            var dict = roles.ToDictionary(r => r.ID);
            foreach (var emp in auths)
            {
                if (dict.TryGetValue(emp.EmployeeID, out var role))
                {
                    emp.Employees = role;
                }
            }
        }
    }
    public class clsAuthsRepositoryLoader : IAuthsRepositoryLoader
    {
        private IEnumerable<IRepositoryBatchsLoader<Auth>> _Loaders;
        public clsAuthsRepositoryLoader(IEnumerable<IRepositoryBatchsLoader<Auth>> Loader)
        {
            _Loaders = Loader;
        }
        public async Task LoadDataAsync(List<Auth> item)
        {
            foreach (var item1 in _Loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsLoginRepositoryReader : clsLoginRepositoryComposition,ILoginRepositoryReader
    {
        private readonly IAuthsRepositoryLoader _Loader;

        public clsLoginRepositoryReader(IOptions<clsMySettings> settings, IAuthsRepositoryLoader loader) : base(settings)
        {
            _Loader = loader;
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
                            Login = GetDataFromDataBase(reader);
                        }
                    }
                }
            }
            if (Login != null)
            {
                await _Loader.LoadDataAsync(new List<Auth> { Login });
            }
            return Login;
        }
    }

    public class clsLoginRepositoryWriter : clsLoginRepositoryComposition, ILoginRepositoryWriter
    {
        public clsLoginRepositoryWriter(IOptions<clsMySettings> settings) : base(settings)
        { }

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

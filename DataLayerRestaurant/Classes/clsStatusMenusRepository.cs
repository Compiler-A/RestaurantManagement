using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusMenus;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes
{ 
    public class clsStatusMenusRepositoryComposition : ICompositionDataBase<StatusMenu>
    {
        public StatusMenu GetDataFromDataBase(SqlDataReader reader)
        {
            return new StatusMenu
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }

    public class clsStatusMenusRepositoryReader : clsStatusMenusRepositoryComposition, IStatusMenusRepositoryReader
    {
        private readonly clsMySettings _Settings;

        public clsStatusMenusRepositoryReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<StatusMenu?> GetDataAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(_Settings.ConnectionString);
            using SqlCommand command = new SqlCommand("StatusMenus.SP_GetStatusMenusByID", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return GetDataFromDataBase(reader);
            }
            return null;
        }

        public async Task<List<StatusMenu>> GetAllDataAsync(int page)
        {
            var list = new List<StatusMenu>();
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_GetAllStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@PageNumber", page);
            command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(GetDataFromDataBase(reader));
            }
            return list;
        }
    }
    
    public class clsStatusMenusRepositoryWriter : clsStatusMenusRepositoryComposition , IStatusMenusRepositoryWriter
    {
        private readonly clsMySettings _Settings;

        public clsStatusMenusRepositoryWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<StatusMenu?> CreateDataAsync(DTOStatusMenusCRequest statusMenu)
        {
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_AddStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Name", statusMenu.Name);
            command.Parameters.AddWithValue("@Description", (object?)statusMenu.Description ?? DBNull.Value);
            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return GetDataFromDataBase(reader);
                }
            }

            return null;
        }

        public async Task<StatusMenu?> UpdateDataAsync(DTOStatusMenusURequest statusMenu)
        {
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_UpdateStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", statusMenu.ID);
            command.Parameters.AddWithValue("@Name", statusMenu.Name);
            command.Parameters.AddWithValue("@Description", (object?)statusMenu.Description ?? DBNull.Value);

            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return GetDataFromDataBase(reader);
                }
            }

            return null;
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_DeleteStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", id);

            await connection.OpenAsync();
            int rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }

    public class clsStatusMenusRepository : IStatusMenusRepository
    {
        IStatusMenusRepositoryReader _IRead;
        IStatusMenusRepositoryWriter _IWrite;

        public clsStatusMenusRepository(IStatusMenusRepositoryReader read, IStatusMenusRepositoryWriter write)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<StatusMenu>> GetAllDataAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }

        public async Task<StatusMenu?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<StatusMenu?> CreateDataAsync(DTOStatusMenusCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }

        public async Task<StatusMenu?> UpdateDataAsync(DTOStatusMenusURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }

}




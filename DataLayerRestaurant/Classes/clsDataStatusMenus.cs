using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusMenus;

namespace DataLayerRestaurant.Classes
{ 
    public class clsCompositionDStatusMenus : ICompositionDataBase<DTOStatusMenus>
    {
        public DTOStatusMenus GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOStatusMenus
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }

    public class clsStatusMenusReader : clsCompositionDStatusMenus, IReadableDStatusMenus
    {
        private readonly clsMySettings _Settings;

        public clsStatusMenusReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOStatusMenus?> GetDataAsync(int id)
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

        public async Task<List<DTOStatusMenus>> GetAllDataAsync(int page)
        {
            var list = new List<DTOStatusMenus>();
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
    
    public class clsStatusMenusWriter : clsCompositionDStatusMenus , IWritableDStatusMenus
    {
        private readonly clsMySettings _Settings;

        public clsStatusMenusWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOStatusMenus?> CreateDataAsync(DTOStatusMenusCRequest statusMenu)
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

        public async Task<DTOStatusMenus?> UpdateDataAsync(DTOStatusMenusURequest statusMenu)
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

    public class clsDataStatusMenus : IDataStatusMenus
    {
        IReadableDStatusMenus _IRead;
        IWritableDStatusMenus _IWrite;

        public clsDataStatusMenus(IReadableDStatusMenus read, IWritableDStatusMenus write)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<DTOStatusMenus>> GetAllStatusMenusAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }

        public async Task<DTOStatusMenus?> GetStatusMenuAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<DTOStatusMenus?> AddStatusMenuAsync(DTOStatusMenusCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }

        public async Task<DTOStatusMenus?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteStatusMenuAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }

}




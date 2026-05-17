using ContractsLayerRestaurant.DTORequest.StatusMenus;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.SQL
{ 

    public class StatusMenusRepositoryReader : IStatusMenusRepositoryReader
    {
        private readonly clsMySettings _Settings;

        public StatusMenusRepositoryReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<List<StatusMenu>> GetAllDataAsync(List<int> Ids)
        {
            List<StatusMenu> result = new List<StatusMenu>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusMenus.SP_GetAllStatusMenusByIds", Connection))
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
                            result.Add(StatusMenuMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
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
                return StatusMenuMapper.ReaderToEntity(reader);
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
                list.Add(StatusMenuMapper.ReaderToEntity(reader));
            }
            return list;
        }
    }
    
    public class StatusMenusRepositoryWriter : IStatusMenusRepositoryWriter
    {
        private readonly clsMySettings _Settings;

        public StatusMenusRepositoryWriter(IOptions<clsMySettings> settings)
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
                    return StatusMenuMapper.ReaderToEntity(reader);
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
                    return StatusMenuMapper.ReaderToEntity(reader);
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


}




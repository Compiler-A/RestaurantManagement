using ContractsLayerRestaurant.DTORequest.Settings;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System.Data;

namespace DataLayerRestaurant.Classes
{

    public class clsSettingsRepositoryReader : ISettingsRepositoryReader
    {

        private readonly clsMySettings _Settings;

        public clsSettingsRepositoryReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }
        public async Task<List<Setting>> GetAllDataAsync(List<int> Ids)
        {
            List<Setting> result = new List<Setting>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_GetAllSettingsByIds", Connection))
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
                            result.Add(SettingMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Setting>> GetAllDataAsync(int page)
        {
            List<Setting> result = new List<Setting>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_GetAllSettings", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(SettingMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<Setting?> GetDataAsync(int ID)
        {
            Setting? result = null;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_GetSettingByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = (SettingMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
        }
    }
    public class clsSettingsRepositoryWriter : ISettingsRepositoryWriter
    {

        private readonly clsMySettings _Settings;
        public clsSettingsRepositoryWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<Setting?> CreateDataAsync(DTOSettingsCRequest dto)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_AddSetting", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", dto.Name);
                    Command.Parameters.AddWithValue("@Value", (object?)dto.Value ?? DBNull.Value);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (SettingMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return null;
        }
        public async Task<Setting?> UpdateDataAsync(DTOSettingsURequest DTO)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_UpdateSetting", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", DTO.Name);
                    Command.Parameters.AddWithValue("@Value", (object?)DTO.Value ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@ID", DTO.ID);
                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (SettingMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return null;
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_DeleteSetting", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);
                    await Connection.OpenAsync();
                    int Row = await Command.ExecuteNonQueryAsync();
                    Deleted = Row > 0;
                }
            }
            return Deleted;
        }
    }

    public class clsSettingsRepository : ISettingsRepository
    {
        private readonly ISettingsRepositoryWriter _Write;
        private readonly ISettingsRepositoryReader _Read;

        public clsSettingsRepository(ISettingsRepositoryWriter write, ISettingsRepositoryReader read)
        {
            _Write = write;
            _Read = read;
        }

        public async Task<List<Setting>> GetAllDataAsync(List<int> Ids)
        {
            return await _Read.GetAllDataAsync(Ids);
        }

        public async Task<List<Setting>> GetAllDataAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }

        public async Task<Setting?> GetDataAsync(int ID)
        { 
            return await _Read.GetDataAsync(ID);
        }

        public async Task<Setting?> CreateDataAsync(DTOSettingsCRequest DTO)
        {

            return await _Write.CreateDataAsync(DTO);
        }

        public async Task<Setting?> UpdateDataAsync(DTOSettingsURequest DTO)
        {

            return await _Write.UpdateDataAsync(DTO);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        { 
            return await _Write.DeleteDataAsync(ID);
        }
    }
}

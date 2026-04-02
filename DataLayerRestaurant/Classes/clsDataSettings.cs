using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public class DTOSettingsCRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }

        public DTOSettingsCRequest(string name, decimal value)
        {
            Name = name;
            Value = value;
        }
    }
    public class DTOSettingsURequest : DTOSettingsCRequest
    {
        public int ID { get; set; }
        public DTOSettingsURequest(int id,string name, decimal value) : base(name, value)
        {
            ID = id;
            Name = name;
            Value = value;
        }
    }

    public class DTOSettings
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }

        public DTOSettings()
        {
            ID = -1;
            Name = string.Empty;
            Value = 0;
        }
        public DTOSettings(int id, string key, string value)
        {
            ID = id;
            Name = key;
            Value = 0;
        }
    }

   

    public class clsCompositionDSettings : ICompositionDataBase <DTOSettings>
    {
        public DTOSettings GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOSettings
            {
                ID = reader.GetInt32(reader.GetOrdinal("SettingID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Value = reader.GetDecimal(reader.GetOrdinal("Value"))
            };
        }
    }

    public class clsSettingsReader : clsCompositionDSettings , IReadableDSettings
    {

        private readonly clsMySettings _Settings;

        public clsSettingsReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<List<DTOSettings>> GetAllDataAsync(int page)
        {
            List<DTOSettings> result = new List<DTOSettings>();
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
                            result.Add(GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<DTOSettings?> GetDataAsync(int ID)
        {
            DTOSettings? result = null;
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
                            result = (GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }
    }
    public class clsSettingsWriter : clsCompositionDSettings , IWritableDSettings
    {

        private readonly clsMySettings _Settings;
        public clsSettingsWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOSettings?> CreateDataAsync(DTOSettingsCRequest dto)
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
                            return (GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            return null;
        }
        public async Task<DTOSettings?> UpdateDataAsync(DTOSettingsURequest DTO)
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
                            return (GetDataFromDataBase(reader));
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

    public class clsDataSettings : IDataSettings
    {
        private readonly IWritableDSettings _Write;
        private readonly IReadableDSettings _Read;

        public clsDataSettings(IWritableDSettings write, IReadableDSettings read)
        {
            _Write = write;
            _Read = read;
        }

        public async Task<List<DTOSettings>> GetAllSettingsAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }

        public async Task<DTOSettings?> GetSettingAsync(int ID)
        { 
            return await _Read.GetDataAsync(ID);
        }

        public async Task<DTOSettings?> AddSettingAsync(DTOSettingsCRequest DTO)
        {

            return await _Write.CreateDataAsync(DTO);
        }

        public async Task<DTOSettings?> UpdateSettingAsync(DTOSettingsURequest DTO)
        {

            return await _Write.UpdateDataAsync(DTO);
        }

        public async Task<bool> DeleteSettingAsync(int ID)
        { 
            return await _Write.DeleteDataAsync(ID);
        }
    }
}

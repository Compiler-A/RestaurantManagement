using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

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

    public interface IReadableSettingsData
    {
        Task<List<DTOSettings>> GetAllSettings(int page);
        Task<DTOSettings?> GetSetting(int ID);
    }
    public interface IWritableSettingsData
    {
        Task<int> Add(DTOSettings setting);
        Task<bool> Update(DTOSettings setting);
        Task<bool> Delete(int ID);
    }

    public interface IDataSettings : IReadableSettingsData, IWritableSettingsData
    { }

    public class clsDataSettings : IDataSettings
    {
        private DTOSettings _GetDataBase(SqlDataReader reader)
        {
            return new DTOSettings
            {
                ID = reader.GetInt32(reader.GetOrdinal("SettingID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Value = reader.GetDecimal(reader.GetOrdinal("Value"))
            };
        }

        public async Task<List<DTOSettings>> GetAllSettings(int page)
        {
            List<DTOSettings> result = new List<DTOSettings>();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_GetAllSettings", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(_GetDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<DTOSettings?> GetSetting(int ID)
        {
            DTOSettings? result = null;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
                            result = (_GetDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Add(DTOSettings DTO)
        {
            int result = -1;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_AddSetting", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", DTO.Name);
                    Command.Parameters.AddWithValue("@Value", (object?)DTO.Value ?? DBNull.Value);
                    SqlParameter output = new SqlParameter("@NewID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(output);
                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();

                    if (output.Value == null || output.Value == DBNull.Value)
                        return -1;

                    result = (int)output.Value;
                }
            }
            return result;
        }

        public async Task<bool> Update(DTOSettings DTO)
        {
            bool Updated = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Settings.SP_UpdateSetting", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", DTO.Name);
                    Command.Parameters.AddWithValue("@Value", (object?)DTO.Value ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@ID", DTO.ID);

                    await Connection.OpenAsync();
                    int Row = await Command.ExecuteNonQueryAsync();

                    Updated = Row > 0;
                }
            }
            return Updated;
        }

        public async Task<bool> Delete(int ID)
        {
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
}

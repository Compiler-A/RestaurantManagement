using DomainLayer.Entities;
using Microsoft.Data.SqlClient;


namespace DataLayerRestaurant.Mapper
{
    public class SettingMapper
    {
        public static Setting ReaderToEntity(SqlDataReader reader)
        {
            return new Setting
            {
                SettingID = reader.GetInt32(reader.GetOrdinal("SettingID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Value = reader.GetDecimal(reader.GetOrdinal("Value"))
            };
        }
    }
}

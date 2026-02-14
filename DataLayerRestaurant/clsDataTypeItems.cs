using Microsoft.Data.SqlClient;
using RestaurantDataLayer;

namespace DataLayerRestaurant
{
    public class DTOTypeItems
    {
        public int TypeItemID { get; set; }
        public string TypeItemName { get; set; }
        public string? TypeItemDescription { get; set; }

        public DTOTypeItems()
        {
            TypeItemID = -1;
            TypeItemName = string.Empty;
            TypeItemDescription = null;
        }

        public DTOTypeItems(int typeItemID, string typeItemName, string? typeItemDescription)
        {
            TypeItemID = typeItemID;
            TypeItemName = typeItemName;
            TypeItemDescription = typeItemDescription;
        }
    }
    public interface IDataTypeItems
    {
        Task<DTOTypeItems?> GetTypeItemById(int typeItemId);
        Task<List<DTOTypeItems>> GetAllTypeItems(int page);
        Task<int> AddTypeItem(DTOTypeItems? typeItem);
        Task<bool> UpdateTypeItem(DTOTypeItems? typeItem);
        Task<bool> DeleteTypeItem(int typeItemID);
    }

    public class clsDataTypeItems : IDataTypeItems
    {
        private static DTOTypeItems _GetTypeItemFromDataBase(SqlDataReader reader)
        {
            return new DTOTypeItems
            {
                TypeItemID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                TypeItemName = reader.GetString(reader.GetOrdinal("Name")),
                TypeItemDescription = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }

        public async Task<List<DTOTypeItems>> GetAllTypeItems(int page)
        {
            List<DTOTypeItems> list = new List<DTOTypeItems>();

            using SqlConnection connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_GetAllTypeItems", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandTimeout = 15
            };

            command.Parameters.AddWithValue("@PageNumber", page);
            command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(_GetTypeItemFromDataBase(reader));
            }

            return list;
        }

        public async Task<DTOTypeItems?> GetTypeItemById(int id)
        {
            DTOTypeItems? typeItem = null;
            using SqlConnection connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_GetTypeItemById", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandTimeout = 15
            };
            command.Parameters.AddWithValue("@TypeItemID", id);

            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                typeItem = _GetTypeItemFromDataBase(reader);
            }
            return typeItem;
        }

        public async Task<int> AddTypeItem(DTOTypeItems? typeItem)
        {
            if (typeItem == null) return -1;

            using SqlConnection connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_AddTypeItem", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandTimeout = 15
            };

            command.Parameters.AddWithValue("@Name", typeItem.TypeItemName);
            command.Parameters.AddWithValue("@Description", (object?)typeItem.TypeItemDescription ?? DBNull.Value);

            SqlParameter outputIdParam = new SqlParameter("@TypeItemID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(outputIdParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            if (outputIdParam.Value == null || outputIdParam.Value == DBNull.Value) return -1;
            return (int)outputIdParam.Value;
        }

        public async Task<bool> UpdateTypeItem(DTOTypeItems? typeItem)
        {
            if (typeItem == null) return false;

            using SqlConnection connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_UpdateTypeItem", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandTimeout = 15
            };

            command.Parameters.AddWithValue("@TypeItemID", typeItem.TypeItemID);
            command.Parameters.AddWithValue("@Name", typeItem.TypeItemName);
            command.Parameters.AddWithValue("@Description", (object?)typeItem.TypeItemDescription ?? DBNull.Value);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTypeItem(int typeItemID)
        {
            using SqlConnection connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_DeleteTypeItem", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandTimeout = 15
            };
            command.Parameters.AddWithValue("@TypeItemID", typeItemID);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System.Runtime;
using System.ComponentModel.DataAnnotations;

namespace DataLayerRestaurant
{
    public class DTOTypeItemsCRequest
    {
        [Required(ErrorMessage ="Name is Required.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOTypeItemsCRequest(string name, string? description)
        {
            Name = name; 
            Description = description; 
        }
    }

    public class DTOTypeItemsURequest : DTOTypeItemsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int ID { get; set; }

        public DTOTypeItemsURequest(int id,  string name, string? description) 
            : base(name, description)
        {
            ID = id;
        }
    }

    public class DTOTypeItems
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is Required.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOTypeItems()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
        }

        public DTOTypeItems(int typeItemID, string typeItemName, string? typeItemDescription)
        {
            ID = typeItemID;
            Name = typeItemName;
            Description = typeItemDescription;
        }
    }



    public class clsCompositionDTypeItems : ICompositionDataBase<DTOTypeItems>
    {
        public DTOTypeItems GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOTypeItems
            {
                ID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }

    public class clsTypeItemsReader : clsCompositionDTypeItems, IReadableDTypeItems
    {
        private readonly clsMySettings _Settings;

        public clsTypeItemsReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }


        public async Task<List<DTOTypeItems>> GetAllDataAsync(int page)
        {
            List<DTOTypeItems> list = new List<DTOTypeItems>();

            using SqlConnection connection = new SqlConnection(_Settings.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_GetAllTypeItems", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandTimeout = 15
            };

            command.Parameters.AddWithValue("@PageNumber", page);
            command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(GetDataFromDataBase(reader));
            }

            return list;
        }

        public async Task<DTOTypeItems?> GetDataAsync(int id)
        {
            DTOTypeItems? typeItem = null;
            using SqlConnection connection = new SqlConnection(_Settings.ConnectionString);
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
                typeItem = GetDataFromDataBase(reader);
            }
            return typeItem;
        }
    }

    public class clsTypeItemsWriter : clsCompositionDTypeItems , IWritableDTypeItems
    {
        private readonly clsMySettings _Settings;

        public clsTypeItemsWriter(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }   

        public async Task<DTOTypeItems?> CreateDataAsync(DTOTypeItemsCRequest typeItem)
        {
            using SqlConnection connection = new SqlConnection(_Settings.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_AddTypeItem", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@Name", typeItem.Name);
            command.Parameters.AddWithValue("@Description", (object?)typeItem.Description ?? DBNull.Value);

            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return GetDataFromDataBase(reader);
            }
            return null;
        }

        public async Task<DTOTypeItems?> UpdateDataAsync(DTOTypeItemsURequest typeItem)
        {

            using SqlConnection connection = new SqlConnection(_Settings.ConnectionString);
            using SqlCommand command = new SqlCommand("TypeItems.SP_UpdateTypeItem", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@TypeItemID", typeItem.ID);
            command.Parameters.AddWithValue("@Name", typeItem.Name);
            command.Parameters.AddWithValue("@Description", (object?)typeItem.Description ?? DBNull.Value);

            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return GetDataFromDataBase(reader);
            }
            return null;
        }

        public async Task<bool> DeleteDataAsync(int typeItemID)
        {
            using SqlConnection connection = new SqlConnection(_Settings.ConnectionString);
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



    public class clsDataTypeItems : IDataTypeItems
    {
        IReadableDTypeItems _IRead;
        IWritableDTypeItems _IWrite;

        public clsDataTypeItems(IReadableDTypeItems Read, IWritableDTypeItems Write)
        {
            _IRead = Read;
            _IWrite = Write;
        }

        public async Task<DTOTypeItems?> GetTypeItemAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }
        public async Task<List<DTOTypeItems>> GetAllTypeItemsAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }

        public async Task<DTOTypeItems?> AddTypeItemAsync(DTOTypeItemsCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }
        public async Task<DTOTypeItems?> UpdateTypeItemAsync(DTOTypeItemsURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteTypeItemAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }

    }

        
}

using ContractsLayerRestaurant.DTORequest.TypeItems;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes
{

    public class clsTypeItemsRepositoryReader : ITypeItemsRepositoryReader
    {
        private readonly clsMySettings _Settings;

        public clsTypeItemsRepositoryReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<List<TypeItem>> GetAllDataAsync(List<int> Ids)
        {
            List<TypeItem> result = new List<TypeItem>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("TypeItems.SP_GetAllTypeItemsByIds", Connection))
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
                            result.Add(TypeItemMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TypeItem>> GetAllDataAsync(int page)
        {
            List<TypeItem> list = new List<TypeItem>();

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
                list.Add(TypeItemMapper.ReaderToEntity(reader));
            }

            return list;
        }

        public async Task<TypeItem?> GetDataAsync(int id)
        {
            TypeItem? typeItem = null;
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
                typeItem = TypeItemMapper.ReaderToEntity(reader);
            }
            return typeItem;
        }
    }

    public class clsTypeItemsRepositoryWriter : ITypeItemsRepositoryWriter
    {
        private readonly clsMySettings _Settings;

        public clsTypeItemsRepositoryWriter(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }   

        public async Task<TypeItem?> CreateDataAsync(DTOTypeItemsCRequest typeItem)
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
                return TypeItemMapper.ReaderToEntity(reader);
            }
            return null;
        }

        public async Task<TypeItem?> UpdateDataAsync(DTOTypeItemsURequest typeItem)
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
                return TypeItemMapper.ReaderToEntity(reader);
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



    public class clsTypeItemsRepository : ITypeItemsRepository
    {
        ITypeItemsRepositoryReader _IRead;
        ITypeItemsRepositoryWriter _IWrite;

        public clsTypeItemsRepository(ITypeItemsRepositoryReader Read, ITypeItemsRepositoryWriter Write)
        {
            _IRead = Read;
            _IWrite = Write;
        }

        public async Task<List<TypeItem>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<TypeItem?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }
        public async Task<List<TypeItem>> GetAllDataAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }

        public async Task<TypeItem?> CreateDataAsync(DTOTypeItemsCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }
        public async Task<TypeItem?> UpdateDataAsync(DTOTypeItemsURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }

    }

        
}

using ContractsLayerRestaurant.DTORequest.MenuItems;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;


namespace DataLayerRestaurant.Classes
{


    public class clsMenuItemsRepositoryReader :  IMenuItemsRepositoryReader
    {
        private readonly clsMySettings _Settings;

        public clsMenuItemsRepositoryReader(IOptions<clsMySettings> settings)
        { 
            _Settings = settings.Value;
        }
        public async Task<MenuItem?> GetDataAsync(int ID)
        {
            MenuItem? menuItem = null;
            using (SqlConnection conn = new SqlConnection(_Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetMenuItemByID", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuItemID", ID);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        menuItem = MenuItemMapper.ReaderToEntityResult(reader);
                }
            }

            return menuItem;
        }
        public async Task<List<MenuItem>> GetAllDataAsync(List<int> Ids)
        {
            List<MenuItem> result = new List<MenuItem>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("MenuItems.SP_GetAllMenuItemsByIds", Connection))
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
                            result.Add(MenuItemMapper.ReaderToEntityResult(reader));
                        }
                    }
                }
            }

            return result;
        }
        public async Task<List<MenuItem>> GetAllDataAsync(int Page)
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            using (SqlConnection conn = new SqlConnection(_Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetAllMenuItems", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Page", Page);
                cmd.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        menuItems.Add(MenuItemMapper.ReaderToEntityResult(reader));
                    }
                }
            }
            return menuItems;
        }

        public async Task<List<MenuItem>> GetAllDataAvailablesAsync()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            using (SqlConnection conn = new SqlConnection(_Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetAllMenuItemsAvailables", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        menuItems.Add(MenuItemMapper.ReaderToEntityResult(reader));
                    }
                }
            }
           
            return menuItems;
        }

        public async Task<List<MenuItem>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            using (SqlConnection conn = new SqlConnection(_Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetFilterTypeItemAndStatusMenuMenuItems", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Page", Request.Page);
                cmd.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);
                cmd.Parameters.AddWithValue("@StatusMenu", Request.StatusMenuID);
                cmd.Parameters.AddWithValue("@TypeItem", Request.TypeItemID);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        menuItems.Add(MenuItemMapper.ReaderToEntityResult(reader));
                    }
                }
            }
            return menuItems;
        }

    }

    public class clsMenuItemsRepositoryWriter : IMenuItemsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        public clsMenuItemsRepositoryWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<MenuItem?> CreateDataAsync(DTOMenuItemsCRequest menuItem)
        {
            MenuItem? createdMenuItem = null;
            using (SqlConnection conn = new SqlConnection(_Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_AddMenuItem", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuItemName", menuItem.Name);
                cmd.Parameters.AddWithValue("@MenuItemDescription", (object?)menuItem.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MenuItemPrice", menuItem.Price);
                cmd.Parameters.AddWithValue("@TypeItemID", menuItem.TypeItemID);
                cmd.Parameters.AddWithValue("@StatusMenuID", menuItem.StatusMenuID);
                cmd.Parameters.AddWithValue("@Image", (object?)menuItem.Image ?? DBNull.Value);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                       createdMenuItem = MenuItemMapper.ReaderToEntityResult(reader);
                }
            }
            if (createdMenuItem == null)
            {
                return null;
            }
            return createdMenuItem;
        }

        public async Task<MenuItem?> UpdateDataAsync(DTOMenuItemsURequest menuItem)
        {
            MenuItem? updatedMenuItem = null;
            using (SqlConnection conn = new SqlConnection(_Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_UpdateMenuItem", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuItemID", menuItem.ID);
                cmd.Parameters.AddWithValue("@MenuItemName", menuItem.Name);
                cmd.Parameters.AddWithValue("@MenuItemDescription", (object?)menuItem.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MenuItemPrice", menuItem.Price);
                cmd.Parameters.AddWithValue("@TypeItemID", menuItem.TypeItemID);
                cmd.Parameters.AddWithValue("@StatusMenuID", menuItem.StatusMenuID);
                cmd.Parameters.AddWithValue("@Image", (object?)menuItem.Image ?? DBNull.Value);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        updatedMenuItem = MenuItemMapper.ReaderToEntityResult(reader);
                }

                if (updatedMenuItem == null)
                {
                    return null;
                }
                return updatedMenuItem;
            }
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_DeleteMenuItem", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuItemID", id);

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }


    public class clsMenuItemsRepository : IMenuItemsRepository
    {

        IMenuItemsRepositoryReader _IRead;
        IMenuItemsRepositoryWriter _IWrite;

        public clsMenuItemsRepository(IMenuItemsRepositoryReader iRead, IMenuItemsRepositoryWriter iWrite)
        {
            _IRead = iRead;
            _IWrite = iWrite;
        }

        public async Task<List<MenuItem>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<List<MenuItem>> GetAllDataAvailablesAsync()
        {
            return await _IRead.GetAllDataAvailablesAsync();
        }
        public async Task<List<MenuItem>> GetAllDataAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }
        
        
        public async Task<List<MenuItem>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
           return await _IRead.GetAllDataFiltersAsync(Request);
        }


        public async Task<MenuItem?> GetDataAsync(int id)
        {
            return await _IRead.GetDataAsync(id);
        }

        public async Task<MenuItem?> CreateDataAsync(DTOMenuItemsCRequest menuItem)
        {
            return await _IWrite.CreateDataAsync(menuItem);
        }

        public async Task<MenuItem?> UpdateDataAsync(DTOMenuItemsURequest menuItem)
        {
           return await _IWrite.UpdateDataAsync(menuItem);
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            return await _IWrite.DeleteDataAsync(id);
        }
    }
}


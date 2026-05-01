using ContractsLayerRestaurant.DTORequest.MenuItems;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System.Data;


namespace DataLayerRestaurant.Classes
{

    public class clsMenuItemsRepositoryCompositon : ICompositionDataBase<MenuItem>
    {
        public MenuItem GetDataFromDataBase(SqlDataReader reader)
        {
            MenuItem menuItem = new MenuItem
            {
                ID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                TypeItemID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                StatusMenuID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image"))
            };
            return menuItem;
        }
    }


    public class clsTypeItemBatchLoader : IRepositoryBatchsLoader<MenuItem>
    {
        private readonly ITypeItemsRepositoryReader _service;

        public clsTypeItemBatchLoader(ITypeItemsRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<MenuItem> menuItem)
        {
            var TypeItemID = menuItem.Select(e => e.TypeItemID).Distinct().ToList();

            if (!TypeItemID.Any())
                return;

            var roles = await _service.GetAllDataAsync(TypeItemID);

            var dict = roles.ToDictionary(r => r.ID);

            foreach (var emp in menuItem)
            {
                if (dict.TryGetValue(emp.TypeItemID, out var role))
                {
                    emp.TypeItems = role;
                }
            }
        }
    }
    public class clsStatusMenuBatchLoader : IRepositoryBatchsLoader<MenuItem>
    {
        private readonly IStatusMenusRepositoryReader _service;

        public clsStatusMenuBatchLoader(IStatusMenusRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<MenuItem> menuItem)
        {
            var StatusMenuID = menuItem.Select(e => e.StatusMenuID).Distinct().ToList();

            if (!StatusMenuID.Any())
                return;

            var roles = await _service.GetAllDataAsync(StatusMenuID);

            var dict = roles.ToDictionary(r => r.ID);

            foreach (var emp in menuItem)
            {
                if (dict.TryGetValue(emp.StatusMenuID, out var role))
                {
                    emp.StatusMenus = role;
                }
            }
        }
    }


    public class clsMenuItemsRepositoryLoader : IMenuItemsRepositoryLoader
    {
        private IEnumerable<IRepositoryBatchsLoader<MenuItem>> _Loaders;
        public clsMenuItemsRepositoryLoader(IEnumerable<IRepositoryBatchsLoader<MenuItem>> Loader)
        {
            _Loaders = Loader;
        }
        public async Task LoadDataAsync(List<MenuItem> item)
        {
            foreach (var item1 in _Loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }



    public class clsMenuItemsRepositoryReader : clsMenuItemsRepositoryCompositon ,IMenuItemsRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private IMenuItemsRepositoryLoader _Loader;

        public clsMenuItemsRepositoryReader(IOptions<clsMySettings> settings, IMenuItemsRepositoryLoader Loader)
        {
            _Settings = settings.Value;
            _Loader = Loader;
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
                        menuItem = GetDataFromDataBase(reader);
                }
            }
            if (menuItem == null)
            {
                return null;
            }
            await _Loader.LoadDataAsync(new List<MenuItem> { menuItem });
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
                            result.Add(GetDataFromDataBase(reader));
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
                        menuItems.Add(GetDataFromDataBase(reader));
                    }
                }
            }
            await _Loader.LoadDataAsync(menuItems);
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
                        menuItems.Add(GetDataFromDataBase(reader));
                    }
                }
            }
           
            await _Loader.LoadDataAsync(menuItems);
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
                        menuItems.Add(GetDataFromDataBase(reader));
                    }
                }
            }
            await _Loader.LoadDataAsync(menuItems);
            return menuItems;
        }

    }

    public class clsMenuItemsRepositoryWriter : clsMenuItemsRepositoryCompositon ,IMenuItemsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        private IMenuItemsRepositoryLoader _Loader;
        public clsMenuItemsRepositoryWriter(IOptions<clsMySettings> settings, IMenuItemsRepositoryLoader loader)
        {
            _Settings = settings.Value;
            _Loader = loader;
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
                       createdMenuItem =  GetDataFromDataBase(reader);
                }
            }
            if (createdMenuItem == null)
            {
                return null;
            }
            await _Loader.LoadDataAsync(new List<MenuItem> { createdMenuItem });
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
                        updatedMenuItem = GetDataFromDataBase(reader);
                }

                if (updatedMenuItem == null)
                {
                    return null;
                }
                await _Loader.LoadDataAsync(new List<MenuItem> { updatedMenuItem });
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


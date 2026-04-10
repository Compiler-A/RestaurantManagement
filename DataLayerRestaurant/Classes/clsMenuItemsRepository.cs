using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.MenuItems;


namespace DataLayerRestaurant.Classes
{

    public class clsMenuItemsRepositoryCompositon : ICompositionDataBase<DTOMenuItems>
    {
        public DTOMenuItems GetDataFromDataBase(SqlDataReader reader)
        {
            DTOMenuItems menuItem = new DTOMenuItems
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


    public class clsMenuItemsRepositoryReader : clsMenuItemsRepositoryCompositon ,IMenuItemsRepositoryReader
    {
        private readonly clsMySettings _Settings;
        public clsMenuItemsRepositoryReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }
        public async Task<DTOMenuItems?> GetDataAsync(int ID)
        {
            DTOMenuItems? menuItem = null;
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
            return menuItem;
        }

        public async Task<List<DTOMenuItems>> GetAllDataAsync(int Page)
        {
            List<DTOMenuItems> menuItems = new List<DTOMenuItems>();
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
            return menuItems;
        }

        public async Task<List<DTOMenuItems>> GetAllDataAvailablesAsync()
        {
            List<DTOMenuItems> menuItems = new List<DTOMenuItems>();
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
            return menuItems;
        }

        public async Task<List<DTOMenuItems>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            List<DTOMenuItems> menuItems = new List<DTOMenuItems>();
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
            return menuItems;
        }

    }

    public class clsMenuItemsRepositoryWriter : clsMenuItemsRepositoryCompositon ,IMenuItemsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        public clsMenuItemsRepositoryWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOMenuItems?> CreateDataAsync(DTOMenuItemsCRequest menuItem)
        {

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
                       return GetDataFromDataBase(reader);
                }
            }
            return null;
        }

        public async Task<DTOMenuItems?> UpdateDataAsync(DTOMenuItemsURequest menuItem)
        {

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
                        return GetDataFromDataBase(reader);
                }
                return null;
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

        public async Task<List<DTOMenuItems>> GetAllMenuItemsAvailablesAsync()
        {
            return await _IRead.GetAllDataAvailablesAsync();
        }
        public async Task<List<DTOMenuItems>> GetAllMenuItemsAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }
        
        
        public async Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
           return await _IRead.GetAllDataFiltersAsync(Request);
        }


        public async Task<DTOMenuItems?> GetMenuItemAsync(int id)
        {
            return await _IRead.GetDataAsync(id);
        }

        public async Task<DTOMenuItems?> AddMenuItemAsync(DTOMenuItemsCRequest menuItem)
        {
            return await _IWrite.CreateDataAsync(menuItem);
        }

        public async Task<DTOMenuItems?> UpdateMenuItemAsync(DTOMenuItemsURequest menuItem)
        {
           return await _IWrite.UpdateDataAsync(menuItem);
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            return await _IWrite.DeleteDataAsync(id);
        }
    }
}


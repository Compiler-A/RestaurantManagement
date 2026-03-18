using Azure;
using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public class DTOMenuItemsCRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int TypeItemID { get; set; }
        public int StatusMenuID { get; set; }
        public string? Image { get; set; }

        public DTOMenuItemsCRequest(string Name, string? Description, decimal Price, int TypeItemID, int StatusMenuID, string? Image)
        {
            this.Name = Name;
            this.Description = Description;
            this.Price = Price;
            this.TypeItemID = TypeItemID;
            this.StatusMenuID = StatusMenuID;
            this.Image = Image;
        }
    }

    public class DTOMenuItemsURequest : DTOMenuItemsCRequest
    {
        public int ID { get; set; }

        public DTOMenuItemsURequest(int ID, string Name, string? Description, decimal Price, int TypeItemID, int StatusMenuID, string? Image)
            : base(Name, Description, Price, TypeItemID, StatusMenuID, Image)
        {
            this.ID = ID;
        }
    }

    public class DTOMenuItems
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int TypeItemID { get; set; }
        public int StatusMenuID { get; set; }
        public string? Image { get; set; }
        public DTOTypeItems? TypeItems { get; set; }
        public DTOStatusMenus? StatusMenus { get; set; }

        public DTOMenuItems(int menuItemID, string menuItemName, string? menuItemDescription, decimal menuItemPrice, int typeItemID, int statusMenuID, string? image)
        {
            ID = menuItemID;
            Name = menuItemName;
            Description = menuItemDescription;
            Price = menuItemPrice;
            TypeItemID = typeItemID;
            StatusMenuID = statusMenuID;
            Image = image;
        }
        public DTOMenuItems()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
            Price = -1;
            TypeItemID = -1;
            StatusMenuID = -1;
            Image = null;
        }
    }


    public class clsCompositionDMenuItems : ICompositionDataBase<DTOMenuItems>
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


    public class clsReadableDMenuItems : clsCompositionDMenuItems ,IReadableDMenuItems
    {
        public async Task<DTOMenuItems?> GetDataAsync(int ID)
        {
            DTOMenuItems? menuItem = null;
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetAllMenuItems", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Page", Page);
                cmd.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

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
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
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

        public async Task<List<DTOMenuItems>> GetAllDataFiltersAsync(int page, int StatusMenuID, int TypeItemID)
        {
            List<DTOMenuItems> menuItems = new List<DTOMenuItems>();
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetFilterTypeItemAndStatusMenuMenuItems", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Page", page);
                cmd.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);
                cmd.Parameters.AddWithValue("@StatusMenu", StatusMenuID);
                cmd.Parameters.AddWithValue("@TypeItem", TypeItemID);
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

    public class clsWritableDMenuItems : clsCompositionDMenuItems ,IWritableDMenuItems
    {
        public async Task<DTOMenuItems?> CreateDataAsync(DTOMenuItemsCRequest menuItem)
        {

            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
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

            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
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


    public class clsDataMenuItems : IDataMenuItems
    {

        IReadableDMenuItems _IRead;
        IWritableDMenuItems _IWrite;

        public clsDataMenuItems(IReadableDMenuItems iRead, IWritableDMenuItems iWrite)
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
        
        
        public async Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(int page, int StatusMenuID, int TypeItemID)
        {
           return await _IRead.GetAllDataFiltersAsync(page, StatusMenuID, TypeItemID);
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


using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public class DTOMenuItem
    {
        public int MenuItemID { get; set; }
        public string MenuItemName { get; set; }
        public string? MenuItemDescription { get; set; }
        public decimal MenuItemPrice { get; set; }
        public int TypeItemID { get; set; }
        public int StatusMenuID { get; set; }
        public string? Image { get; set; }
        public DTOTypeItems? TypeItems { get; set; }
        public DTOStatusMenus? StatusMenus { get; set; }

        public DTOMenuItem(int menuItemID, string menuItemName, string? menuItemDescription, decimal menuItemPrice, int typeItemID, int statusMenuID, string? image)
        {
            MenuItemID = menuItemID;
            MenuItemName = menuItemName;
            MenuItemDescription = menuItemDescription;
            MenuItemPrice = menuItemPrice;
            TypeItemID = typeItemID;
            StatusMenuID = statusMenuID;
            Image = image;
        }
        public DTOMenuItem()
        {
            MenuItemID = -1;
            MenuItemName = string.Empty;
            MenuItemDescription = null;
            MenuItemPrice = -1;
            TypeItemID = -1;
            StatusMenuID = -1;
            Image = null;
        }
    }

    public interface IDataMenuItems
    {
        Task<DTOMenuItem?> GetMenuItemByID(int id);
        Task<List<DTOMenuItem>> GetAllMenuItems(int page);
        Task<int> AddMenuItem(DTOMenuItem menuItem);
        Task<bool> UpdateMenuItem(DTOMenuItem menuItem);
        Task<bool> DeleteMenuItem(int id);
    }







    public class clsDataMenuItems : IDataMenuItems
    {

        
        private async Task<DTOMenuItem> MapReaderToMenuItem(SqlDataReader reader)
        {
            DTOMenuItem menuItem = new DTOMenuItem
            {
                MenuItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                MenuItemName = reader.GetString(reader.GetOrdinal("Name")),
                MenuItemDescription = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                MenuItemPrice = reader.GetDecimal(reader.GetOrdinal("Price")),
                TypeItemID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                StatusMenuID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image"))
            };
            return menuItem;
        }

        public async Task<List<DTOMenuItem>> GetAllMenuItems(int page)
        {
            List<DTOMenuItem> menuItems = new List<DTOMenuItem>();
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetAllMenuItems", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Page", page);
                cmd.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        menuItems.Add(await MapReaderToMenuItem(reader));
                    }
                }
            }
            return menuItems;
        }

        public async Task<DTOMenuItem?> GetMenuItemByID(int id)
        {
            DTOMenuItem? menuItem = null;
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_GetMenuItemByID", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuItemID", id);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        menuItem = await MapReaderToMenuItem(reader);
                }
            }
            return menuItem;
        }

        public async Task<int> AddMenuItem(DTOMenuItem menuItem)
        {
            if (menuItem == null) throw new ArgumentNullException(nameof(menuItem));

            int newID = -1;
            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_AddMenuItem", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuItemName", menuItem.MenuItemName);
                cmd.Parameters.AddWithValue("@MenuItemDescription", (object?)menuItem.MenuItemDescription ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MenuItemPrice", menuItem.MenuItemPrice);
                cmd.Parameters.AddWithValue("@TypeItemID", menuItem.TypeItemID);
                cmd.Parameters.AddWithValue("@StatusMenuID", menuItem.StatusMenuID);
                cmd.Parameters.AddWithValue("@Image", (object?)menuItem.Image ?? DBNull.Value);

                SqlParameter outputIdParam = new SqlParameter("@NewMenuItemID", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                if (outputIdParam.Value != null && outputIdParam.Value != DBNull.Value)
                    newID = (int)outputIdParam.Value;
            }
            return newID;
        }

        public async Task<bool> UpdateMenuItem(DTOMenuItem menuItem)
        {
            if (menuItem == null) throw new ArgumentNullException(nameof(menuItem));

            using (SqlConnection conn = new SqlConnection(clsDataAccessLayer.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("MenuItems.SP_UpdateMenuItem", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuItemID", menuItem.MenuItemID);
                cmd.Parameters.AddWithValue("@MenuItemName", menuItem.MenuItemName);
                cmd.Parameters.AddWithValue("@MenuItemDescription", (object?)menuItem.MenuItemDescription ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MenuItemPrice", menuItem.MenuItemPrice);
                cmd.Parameters.AddWithValue("@TypeItemID", menuItem.TypeItemID);
                cmd.Parameters.AddWithValue("@StatusMenuID", menuItem.StatusMenuID);
                cmd.Parameters.AddWithValue("@Image", (object?)menuItem.Image ?? DBNull.Value);

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteMenuItem(int id)
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
}


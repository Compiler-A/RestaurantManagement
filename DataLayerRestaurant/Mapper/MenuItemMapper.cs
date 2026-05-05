using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Mapper
{
    public class MenuItemMapper
    {
        public static MenuItem ReaderToEntity(SqlDataReader reader)
        {
            MenuItem menuItem = new MenuItem
            {
                ID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Name = reader.GetString(reader.GetOrdinal("ItemName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                TypeItemID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                StatusMenuID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image"))
            };
            return menuItem;
        }

        public static MenuItem ReaderToEntityResult(SqlDataReader reader)
        {
            MenuItem menuItem = new MenuItem
            {
                ID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Name = reader.GetString(reader.GetOrdinal("ItemName")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                TypeItemID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                StatusMenuID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                TypeItems = new TypeItem
                {
                    ID = reader.GetInt32(reader.GetOrdinal("TypeItemID")),
                    Name = reader.GetString(reader.GetOrdinal("TypeName"))
                },
                StatusMenus = new StatusMenu
                {
                    ID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                    Name = reader.GetString(reader.GetOrdinal("StatusMenuName"))
                }
            };
            return menuItem;
        }
    }
}

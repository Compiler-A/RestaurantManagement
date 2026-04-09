using ContractsLayerRestaurant.DTOs.StatusMenus;
using ContractsLayerRestaurant.DTOs.TypeItems;

namespace ContractsLayerRestaurant.DTOs.MenuItems
{
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
}

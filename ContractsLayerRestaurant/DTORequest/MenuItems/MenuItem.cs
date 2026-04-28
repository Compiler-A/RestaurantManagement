using ContractsLayerRestaurant.DTORequest.StatusMenus;
using ContractsLayerRestaurant.DTORequest.TypeItems;

namespace ContractsLayerRestaurant.DTORequest.MenuItems
{
    public class MenuItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int TypeItemID { get; set; }
        public int StatusMenuID { get; set; }

        public string? Image { get; set; }
        public TypeItem? TypeItems { get; set; }
        public StatusMenu? StatusMenus { get; set; }
    }
}

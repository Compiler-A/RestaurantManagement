using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Mapper
{
    public static class MenuItemMapper
    {
        public static DTOMenuItemResponse ToResponse(this MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));
            return new DTOMenuItemResponse
            {
                ID = menuItem.ItemID,
                Name = menuItem.ItemName,
                Price = menuItem.Price,
                Image = menuItem.Image,
                TypeName = menuItem.TypeItems?.TypeName ?? string.Empty,
                StatusMenuName = menuItem.StatusMenus?.StatusMenuName ?? string.Empty
            };
        }
    }
}

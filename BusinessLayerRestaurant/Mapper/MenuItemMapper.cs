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
                ID = menuItem.ID,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Image = menuItem.Image,
                TypeName = menuItem.TypeItems?.Name ?? string.Empty,
                StatusMenuName = menuItem.StatusMenus?.Name ?? string.Empty
            };
        }
    }
}

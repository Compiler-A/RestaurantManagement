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
                Description = menuItem.Description,
                Price = menuItem.Price,
                TypeItemID = menuItem.TypeItemID,
                StatusMenuID = menuItem.StatusMenuID,
                Image = menuItem.Image,
                TypeItems = menuItem.TypeItems?.ToResponse(),
                StatusMenus = menuItem.StatusMenus?.ToResponse()
            };
        }
    }
}

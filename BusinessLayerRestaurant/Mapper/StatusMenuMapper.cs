

using ContractsLayerRestaurant.DTORequest.StatusMenus;
using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Mapper
{
    public static class StatusMenuMapper
    {
        public static DTOStatusMenuResponse ToResponse(this StatusMenu statusMenu)
        {
            if (statusMenu == null)
                throw new ArgumentNullException(nameof(statusMenu));
            return new DTOStatusMenuResponse
            {
                ID = statusMenu.ID,
                Name = statusMenu.Name,
                Description = statusMenu.Description
            };
        }
    }
}

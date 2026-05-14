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
                ID = statusMenu.StatusMenuID,
                Name = statusMenu.StatusMenuName,
                Description = statusMenu.Description
            };
        }
    }
}

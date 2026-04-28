
using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Mapper
{
    public static class StatusOrderMapper
    {
        public static DTOStatusOrderResponse ToResponse(this StatusOrder statusOrder)
        {
            if (statusOrder == null)
                throw new ArgumentNullException(nameof(statusOrder));
            return new DTOStatusOrderResponse
            {
                ID = statusOrder.ID,
                Name = statusOrder.Name,
            };
        }
    }
}

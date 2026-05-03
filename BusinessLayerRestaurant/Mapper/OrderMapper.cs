

using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Mapper
{
    public static class OrderMapper
    {
        public static DTOOrderResponse ToResponse(this Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            return new DTOOrderResponse
            {
                ID = order.ID,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                UserName = order.employees?.UserName ?? string.Empty,
                tableNumber = order.tables?.Name ?? string.Empty,
                statusOrderName = order.statusOrders?.Name ?? string.Empty,
                Details = order.Details?.Select(d => d.ToResponse()).ToList() ?? new List<DTOOrderDetailResponse>()
            };
        }
    }
}



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
                ID = order.OrderID,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                UserName = order.Employee?.Username ?? string.Empty,
                tableNumber = order.Table?.TableNumber ?? string.Empty,
                statusOrderName = order.StatusOrder?.StatusOrderName ?? string.Empty,
                Details = order.OrderDetails?.Select(d => d.ToResponse()).ToList() ?? new List<DTOOrderDetailResponse>()
            };
        }
    }
}



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
                EmployeeID = order.EmployeeID,
                TableID = order.TableID,
                TotalAmount = order.TotalAmount,
                StatusOrderID = order.StatusOrderID,
                OrderDate = order.OrderDate,
                employees = order.employees?.ToResponse(),
                tables = order.tables?.ToResponse(),
                statusOrders = order.statusOrders?.ToResponse()
            };
        }
    }
}

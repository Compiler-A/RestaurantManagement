using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Mapper
{
    public static class OrderDetailMapper
    {
        public static DTOOrderDetailResponse ToResponse(this OrderDetail orderDetail)
        {
            if (orderDetail == null)
                throw new ArgumentNullException(nameof(orderDetail));
            return new DTOOrderDetailResponse
            {
                ID = orderDetail.OrderDetailID,
                Quantity = orderDetail.Quantity,
                SubTotal = orderDetail.SubTotal,
                OrderID = orderDetail.Order!.OrderID,
                ItemName = orderDetail.Item!.ItemName
            };
        }
    }
}

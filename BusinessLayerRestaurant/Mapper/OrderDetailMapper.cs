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
                ID = orderDetail.ID,
                Quantity = orderDetail.Quantity,
                SubTotal = orderDetail.SubTotal,
                OrderID = orderDetail.Order!.ID,
                ItemName = orderDetail.Item!.Name
            };
        }
    }
}

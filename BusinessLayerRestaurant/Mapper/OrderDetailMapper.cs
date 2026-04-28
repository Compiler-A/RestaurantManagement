using ContractsLayerRestaurant.DTORequest.OrderDetails;
using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OrderID = orderDetail.OrderID,
                ItemID = orderDetail.ItemID,
                Quantity = orderDetail.Quantity,
                SubTotal = orderDetail.SubTotal,
                Order = orderDetail.Order?.ToResponse(),
                Item = orderDetail.Item?.ToResponse()
            };
        }
    }
}

using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOResponse;
using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{
    
    public interface IOrderDetailsServiceContainer : IServiceContainer<IOrderDetailsRepository>
    {
        IOrdersService IBusinessOrder { get; set; }
        IMenuItemsService IBusinessMenuItem { get; set; }
    }



    public interface IOrderDetailsServiceContainers
    {
        IOrdersService IOrder { get; set; }
        IMenuItemsService IMenuItem { get; set; }
    }

    public interface IOrderDetailsServiceReader : IServiceReader<OrderDetail>
    {
        Task<List<OrderDetail>> GetAllByOrderIDAsync(int orderID);
    }

    public interface IOrderDetailsServiceWriter
       : IServiceWriter<OrderDetail, DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    { }


    public interface ICRUDOrderDetailsService : IOrderDetailsServiceWriter, IOrderDetailsServiceReader
    { }

    public interface IOrderDetailsService : ICRUDOrderDetailsService, IOrderDetailsServiceContainers
    {
    }

}

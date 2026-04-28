using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{
    
    public interface IOrderDetailsServiceContainer : IServiceContainer<IOrderDetailsRepository>
    {
        IOrdersService IBusinessOrder { get; set; }
        IMenuItemsService IBusinessMenuItem { get; set; }
    }

    public interface IOrderDetailsServiceComposition
    {
        Task LoadDataAsync(OrderDetail item);
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

    public interface IOrderDetailsServiceReadable
    {
        Task<List<OrderDetail>> GetAllOrderDetailsAsync(int page);
        Task<List<OrderDetail>> GetAllOrderDetailsByOrderIDAsync(int orderID);
        Task<OrderDetail?> GetOrderDetailAsync(int page);
    }
    public interface IOrderDetailsServiceWritable
    {
        Task<OrderDetail?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request);
        Task<OrderDetail?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request);
        Task<bool> DeleteOrderDetailAsync(int ID);
    }
    public interface ICRUDOrderDetailsService : IOrderDetailsServiceWritable, IOrderDetailsServiceReadable
    { }

    public interface IOrderDetailsService : ICRUDOrderDetailsService, IOrderDetailsServiceContainers
    {
    }

}

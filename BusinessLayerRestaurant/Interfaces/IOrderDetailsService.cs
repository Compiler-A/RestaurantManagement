using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.OrderDetails;

namespace BusinessLayerRestaurant.Interfaces
{
    
    public interface IOrderDetailsServiceContainer : IInterfaceBase<IOrderDetailsRepository>
    {
        IOrdersService IBusinessOrder { get; set; }
        IMenuItemsService IBusinessMenuItem { get; set; }
    }

    public interface IOrderDetailsServiceComposition
    {
        Task LoadDataAsync(DTOOrderDetails item);
    }

    public interface IOrderDetailsServiceContainers
    {
        IOrdersService IOrder { get; set; }
        IMenuItemsService IMenuItem { get; set; }
    }

    public interface IOrderDetailsServiceReader : IReadableBusinessBase<DTOOrderDetails>
    {
        Task<List<DTOOrderDetails>> GetAllByOrderIDAsync(int orderID);
    }

    public interface IOrderDetailsServiceWriter
       : IWritableBusinessBase<DTOOrderDetails, DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    { }

    public interface IOrderDetailsServiceReadable
    {
        Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page);
        Task<List<DTOOrderDetails>> GetAllOrderDetailsByOrderIDAsync(int orderID);
        Task<DTOOrderDetails?> GetOrderDetailAsync(int page);
    }
    public interface IOrderDetailsServiceWritable
    {
        Task<DTOOrderDetails?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request);
        Task<DTOOrderDetails?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request);
        Task<bool> DeleteOrderDetailAsync(int ID);
    }
    public interface ICRUDOrderDetailsService : IOrderDetailsServiceWritable, IOrderDetailsServiceReadable
    { }

    public interface IOrderDetailsService : ICRUDOrderDetailsService, IOrderDetailsServiceContainers
    {
    }

}

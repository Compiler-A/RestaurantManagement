using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.OrderDetails;

namespace BusinessLayerRestaurant.Interfaces
{
    
    public interface IInterfaceBOrderDetails : IInterfaceBase<IDataOrderDetails>
    {
        IBusinessOrders IBusinessOrder { get; set; }
        IBusinessMenuItems IBusinessMenuItem { get; set; }
    }

    public interface ICompositionBOrderDetails
    {
        Task LoadDataAsync(DTOOrderDetails item);
    }

    public interface IInterfaceBusinessOrderDetails
    {
        IBusinessOrders IOrder { get; set; }
        IBusinessMenuItems IMenuItem { get; set; }
    }

    public interface IReadableBOrderDetails : IReadableBusinessBase<DTOOrderDetails>
    {
        Task<List<DTOOrderDetails>> GetAllByOrderIDAsync(int orderID);
    }

    public interface IWritableBOrderDetails
       : IWritableBusinessBase<DTOOrderDetails, DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    { }

    public interface IReadableBusinessOrderDetails
    {
        Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page);
        Task<List<DTOOrderDetails>> GetAllOrderDetailsByOrderIDAsync(int orderID);
        Task<DTOOrderDetails?> GetOrderDetailAsync(int page);
    }
    public interface IWritableBusinessOrderDetails
    {
        Task<DTOOrderDetails?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request);
        Task<DTOOrderDetails?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request);
        Task<bool> DeleteOrderDetailAsync(int ID);
    }
    public interface ICRUDBusinessOrderDetails : IWritableBusinessOrderDetails, IReadableBusinessOrderDetails
    { }

    public interface IBusinessOrderDetails : ICRUDBusinessOrderDetails, IInterfaceBusinessOrderDetails
    {
    }

}

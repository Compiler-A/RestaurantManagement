using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Orders;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IReadableBOrders : IReadableBusinessBase<DTOOrders>
    {
        Task<List<DTOOrders>?> GetFilterAsync(DTOOrderFilterRequest Request);
    }
    public interface IWritableBOrders : IWritableBusinessBase<DTOOrders, DTOOrderCRequest, DTOOrderURequest>
    { }

    public interface IInterfaceBOrders : IInterfaceBase<IDataOrders>
    {
        IBusinessStatusOrders IBusinessStatusOrder { get; set; }
        IBusinessEmployees IBusinessEmployee { get; set; }
        IBusinessTables IBusinessTable { get; set; }
    }

    public interface ICompositionBOrders
    {
        Task LoadDataAsync(DTOOrders item);
    }


    public interface IInterfaceBusinessOrders
    {
        IBusinessStatusOrders IStatusOrder { get; set; }
        IBusinessEmployees IEmployee { get; set; }
        IBusinessTables ITable { get; set; }
    }
    public interface IReadableBusinessOrders
    {
        Task<List<DTOOrders>> GetAllOrdersAsync(int page);
        Task<DTOOrders?> GetOrderAsync(int ID);
        Task<List<DTOOrders>?> GetFilterOrdersAsync(DTOOrderFilterRequest Request);
    }
    public interface IWritableBusinessOrders
    {
        Task<DTOOrders?> AddOrderAsync(DTOOrderCRequest Request);
        Task<DTOOrders?> UpdateOrderAsync(DTOOrderURequest Request);
        Task<bool> DeleteOrderAsync(int ID);
    }

    public interface ICRUDBusinessOrders : IWritableBusinessOrders, IReadableBusinessOrders
    { }
    public interface IBusinessOrders : ICRUDBusinessOrders, IInterfaceBusinessOrders
    { }

}

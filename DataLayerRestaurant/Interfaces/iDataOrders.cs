using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.Orders;

namespace DataLayerRestaurant.Interfaces
{
    public interface IReadableDOrders : IReadableDataBase<DTOOrders>
    {
        Task<List<DTOOrders>?> GetFilterDataAsync(DTOOrderFilterRequest OrderFilterRequest);

    }
    public interface IWritableDOrders : IWritableDataBase<DTOOrders,DTOOrderCRequest, DTOOrderURequest>
    { }
    public interface IReadableDataOrders
    {
        Task<List<DTOOrders>> GetAllOrdersAsync(int page);
        Task<DTOOrders?> GetOrderAsync(int ID);
        Task<List<DTOOrders>?> GetFilterOrderAsync(DTOOrderFilterRequest OrderFilterRequest);
    }
    public interface IWritableDataOrders
    {
        Task<DTOOrders?> AddOrderAsync(DTOOrderCRequest order);
        Task<DTOOrders?> UpdateOrderAsync(DTOOrderURequest order);
        Task<bool> DeleteOrderAsync(int id);
    }
    public interface IDataOrders : IReadableDataOrders, IWritableDataOrders
    { }
}

using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.Orders;

namespace DataLayerRestaurant.Interfaces
{
    public interface IOrdersRepositoryReader : IReadableDataBase<DTOOrders>
    {
        Task<List<DTOOrders>?> GetFilterDataAsync(DTOOrderFilterRequest OrderFilterRequest);

    }
    public interface IOrdersRepositoryWriter : IWritableDataBase<DTOOrders,DTOOrderCRequest, DTOOrderURequest>
    { }
    public interface IOrdersRepositoryReadable
    {
        Task<List<DTOOrders>> GetAllOrdersAsync(int page);
        Task<DTOOrders?> GetOrderAsync(int ID);
        Task<List<DTOOrders>?> GetFilterOrderAsync(DTOOrderFilterRequest OrderFilterRequest);
    }
    public interface IOrdersRepositoryWritable
    {
        Task<DTOOrders?> AddOrderAsync(DTOOrderCRequest order);
        Task<DTOOrders?> UpdateOrderAsync(DTOOrderURequest order);
        Task<bool> DeleteOrderAsync(int id);
    }
    public interface IOrdersRepository : IOrdersRepositoryReadable, IOrdersRepositoryWritable
    { }
}

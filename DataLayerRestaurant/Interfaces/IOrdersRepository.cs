using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.Orders;

namespace DataLayerRestaurant.Interfaces
{
    public interface IOrdersRepositoryReader : IReadableDataBase<Order>
    {
        Task<List<Order>?> GetFilterDataAsync(DTOOrderFilterRequest OrderFilterRequest);

    }
    public interface IOrdersRepositoryWriter : IWritableDataBase<Order,DTOOrderCRequest, DTOOrderURequest>
    { }
    public interface IOrdersRepositoryReadable
    {
        Task<List<Order>> GetAllOrdersAsync(int page);
        Task<Order?> GetOrderAsync(int ID);
        Task<List<Order>?> GetFilterOrderAsync(DTOOrderFilterRequest OrderFilterRequest);
    }
    public interface IOrdersRepositoryWritable
    {
        Task<Order?> AddOrderAsync(DTOOrderCRequest order);
        Task<Order?> UpdateOrderAsync(DTOOrderURequest order);
        Task<bool> DeleteOrderAsync(int id);
    }
    public interface IOrdersRepository : IOrdersRepositoryReadable, IOrdersRepositoryWritable
    { }
}

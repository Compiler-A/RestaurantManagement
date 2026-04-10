using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.StatusOrders;

namespace DataLayerRestaurant.Interfaces
{
    public interface IStatusOrdersRepositoryReader : IReadableDataBase<DTOStatusOrders>
    { }
    public interface IStatusOrdersRepositoryWriter : IWritableDataBase<DTOStatusOrders,DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }

    public interface IStatusOrdersRepositoryReadable
    {
        Task<DTOStatusOrders?> GetStatusOrderAsync(int ID);
        Task<List<DTOStatusOrders>> GetAllStatusOrdersAsync(int Page);
    }
    public interface IStatusOrdersRepositoryWritable
    {
        Task<DTOStatusOrders?> AddStatusOrderAsync(DTOStatusOrdersCRequest Request);
        Task<DTOStatusOrders?> UpdateStatusOrderAsync(DTOStatusOrdersURequest Request);
        Task<bool> DeleteStatusOrderAsync(int ID);
    }
    public interface IStatusOrdersRepository : IStatusOrdersRepositoryReadable, IStatusOrdersRepositoryWritable { }

}

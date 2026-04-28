using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IStatusOrdersRepositoryReader : IReadableDataBase<StatusOrder>
    { }
    public interface IStatusOrdersRepositoryWriter : IWritableDataBase<StatusOrder,DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }

    public interface IStatusOrdersRepositoryReadable
    {
        Task<StatusOrder?> GetStatusOrderAsync(int ID);
        Task<List<StatusOrder>> GetAllStatusOrdersAsync(int Page);
    }
    public interface IStatusOrdersRepositoryWritable
    {
        Task<StatusOrder?> AddStatusOrderAsync(DTOStatusOrdersCRequest Request);
        Task<StatusOrder?> UpdateStatusOrderAsync(DTOStatusOrdersURequest Request);
        Task<bool> DeleteStatusOrderAsync(int ID);
    }
    public interface IStatusOrdersRepository : IStatusOrdersRepositoryReadable, IStatusOrdersRepositoryWritable { }

}

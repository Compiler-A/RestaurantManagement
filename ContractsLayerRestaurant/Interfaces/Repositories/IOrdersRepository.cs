using ContractsLayerRestaurant.DTORequest.Orders;
using DomainLayer.Entities;

namespace ContractsLayerRestaurant.Interfaces.Repositories
{
    public interface IOrderDetailBatchLoader
    {
        Task LoadBatchAsync(List<Order> orders);
    }

    public interface IOrdersRepositoryReader : IRepositoryReader<Order>
    {
        Task<List<Order>?> GetFilterDataAsync(DTOOrderFilterRequest OrderFilterRequest);

    }
    public interface IOrdersRepositoryWriter : IRepositoryWriter<Order,DTOOrderCRequest, DTOOrderURequest>
    { }

    public interface IOrdersRepository : IOrdersRepositoryReader, IOrdersRepositoryWriter
    { }
}

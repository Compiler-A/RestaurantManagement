using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.Orders;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IOrdersRepositoryReader : IRepositoryReader<Order>
    {
        Task<List<Order>?> GetFilterDataAsync(DTOOrderFilterRequest OrderFilterRequest);

    }
    public interface IOrdersRepositoryWriter : IRepositoryWriter<Order,DTOOrderCRequest, DTOOrderURequest>
    { }

    public interface IOrdersRepository : IOrdersRepositoryReader, IOrdersRepositoryWriter
    { }
}

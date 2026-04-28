using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IStatusOrdersRepositoryReader : IRepositoryReader<StatusOrder>
    { }
    public interface IStatusOrdersRepositoryWriter : IRepositoryWriter<StatusOrder,DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }

    public interface IStatusOrdersRepository : IStatusOrdersRepositoryReader, IStatusOrdersRepositoryWriter { }

}

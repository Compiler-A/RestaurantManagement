using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DomainLayer.Entities;

namespace ContractsLayerRestaurant.Interfaces.Repositories
{
    public interface IStatusOrdersRepositoryReader : IRepositoryReader<StatusOrder>
    { }
    public interface IStatusOrdersRepositoryWriter : IRepositoryWriter<StatusOrder,DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }

    public interface IStatusOrdersRepository : IStatusOrdersRepositoryReader, IStatusOrdersRepositoryWriter { }

}

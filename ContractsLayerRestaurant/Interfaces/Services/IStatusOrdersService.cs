using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DomainLayer.Entities;
using ContractsLayerRestaurant.Interfaces.Repositories;

namespace ContractsLayerRestaurant.Interfaces.Services
{
    public interface IStatusOrdersServiceReader : IServiceReader<StatusOrder>
    { }
    public interface IStatusOrdersServiceWriter :
        IServiceWriter<StatusOrder, DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }


    public interface IStatusOrdersServiceContainer : IServiceContainer<IStatusOrdersRepository>
    { }

    public interface ICRUDStatusOrdersService : IStatusOrdersServiceWriter, IStatusOrdersServiceReader
    { }

    public interface IStatusOrdersService : ICRUDStatusOrdersService
    { }
}

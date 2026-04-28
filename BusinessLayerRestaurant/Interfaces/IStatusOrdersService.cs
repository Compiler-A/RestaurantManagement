using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Interfaces
{
    public interface IStatusOrdersServiceReader : IServiceReader<StatusOrder>
    { }
    public interface IWritableBStatusOrders :
        IServiceWriter<StatusOrder, DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }


    public interface IStatusOrdersServiceContainer : IServiceContainer<IStatusOrdersRepository>
    { }

    public interface IStatusOrdersServiceReadable
    {
        Task<List<StatusOrder>> GetAllStatusOrdersAsync(int Page);
        Task<StatusOrder?> GetStatusOrdersAsync(int ID);
    }

    public interface IStatusOrdersServiceWritable
    {
        Task<StatusOrder?> AddStatusOrdersAsync(DTOStatusOrdersCRequest Request);
        Task<StatusOrder?> UpdateStatusOrdersAsync(DTOStatusOrdersURequest Request);
        Task<bool> DeleteStatusOrdersAsync(int ID);
    }
    public interface ICRUDStatusOrdersService : IStatusOrdersServiceWritable, IStatusOrdersServiceReadable
    { }

    public interface IStatusOrdersService : ICRUDStatusOrdersService
    { }
}

using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Interfaces
{
    public interface IStatusOrdersServiceReader : IReadableBusinessBase<StatusOrder>
    { }
    public interface IWritableBStatusOrders :
        IWritableBusinessBase<StatusOrder, DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }


    public interface IStatusOrdersServiceContainer : IInterfaceBase<IStatusOrdersRepository>
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

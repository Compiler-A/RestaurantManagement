using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusOrders;


namespace BusinessLayerRestaurant.Interfaces
{
    public interface IStatusOrdersServiceReader : IReadableBusinessBase<DTOStatusOrders>
    { }
    public interface IWritableBStatusOrders :
        IWritableBusinessBase<DTOStatusOrders, DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }


    public interface IStatusOrdersServiceContainer : IInterfaceBase<IStatusOrdersRepository>
    { }

    public interface IStatusOrdersServiceReadable
    {
        Task<List<DTOStatusOrders>> GetAllStatusOrdersAsync(int Page);
        Task<DTOStatusOrders?> GetStatusOrdersAsync(int ID);
    }

    public interface IStatusOrdersServiceWritable
    {
        Task<DTOStatusOrders?> AddStatusOrdersAsync(DTOStatusOrdersCRequest Request);
        Task<DTOStatusOrders?> UpdateStatusOrdersAsync(DTOStatusOrdersURequest Request);
        Task<bool> DeleteStatusOrdersAsync(int ID);
    }
    public interface ICRUDStatusOrdersService : IStatusOrdersServiceWritable, IStatusOrdersServiceReadable
    { }

    public interface IStatusOrdersService : ICRUDStatusOrdersService
    { }
}

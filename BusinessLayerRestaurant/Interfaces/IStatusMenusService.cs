using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusMenus;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface IStatusMenusServiceContainer : IServiceContainer<IStatusMenusRepository>
    { }

    public interface IStatusMenusServiceReader : IServiceReader<StatusMenu>
    { }

    public interface IStatusMenusServiceWriter 
        : IServiceWriter<StatusMenu, DTOStatusMenusCRequest, DTOStatusMenusURequest>
    { }

    public interface IStatusMenusServiceReadable
    {
        Task<List<StatusMenu>> GetAllStatusMenusAsync(int page);
        Task<StatusMenu?> GetStatusMenuAsync(int id);
    }

    public interface IStatusMenusServiceWritable
    {
        Task<StatusMenu?> AddStatusMenuAsync(DTOStatusMenusCRequest Request);
        Task<StatusMenu?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request);
        Task<bool> DeleteStatusMenuAsync(int ID);
    }
    public interface ICRUDStatusMenusService : IStatusMenusServiceReadable , IStatusMenusServiceWritable
    { }

    public interface IStatusMenusService : ICRUDStatusMenusService
    { }

}

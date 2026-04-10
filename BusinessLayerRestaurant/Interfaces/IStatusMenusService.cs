using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusMenus;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface IStatusMenusServiceContainer : IInterfaceBase<IStatusMenusRepository>
    { }

    public interface IStatusMenusServiceReader : IReadableBusinessBase<DTOStatusMenus>
    { }

    public interface IStatusMenusServiceWriter 
        : IWritableBusinessBase<DTOStatusMenus, DTOStatusMenusCRequest, DTOStatusMenusURequest>
    { }

    public interface IStatusMenusServiceReadable
    {
        Task<List<DTOStatusMenus>> GetAllStatusMenusAsync(int page);
        Task<DTOStatusMenus?> GetStatusMenuAsync(int id);
    }

    public interface IStatusMenusServiceWritable
    {
        Task<DTOStatusMenus?> AddStatusMenuAsync(DTOStatusMenusCRequest Request);
        Task<DTOStatusMenus?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request);
        Task<bool> DeleteStatusMenuAsync(int ID);
    }
    public interface ICRUDStatusMenusService : IStatusMenusServiceReadable , IStatusMenusServiceWritable
    { }

    public interface IStatusMenusService : ICRUDStatusMenusService
    { }

}

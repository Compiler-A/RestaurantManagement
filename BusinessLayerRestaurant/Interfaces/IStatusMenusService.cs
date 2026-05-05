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

    public interface ICRUDStatusMenusService : IStatusMenusServiceReader , IStatusMenusServiceWriter
    { }

    public interface IStatusMenusService : ICRUDStatusMenusService
    { }

}

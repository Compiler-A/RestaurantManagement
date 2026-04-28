using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.StatusMenus;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{

    public interface IStatusMenusRepositoryReader : IRepositoryReader<StatusMenu>
    { }

    public interface IStatusMenusRepositoryWriter 
        : IRepositoryWriter<StatusMenu, DTOStatusMenusCRequest, DTOStatusMenusURequest>
    { }

    public interface IStatusMenusRepository : IStatusMenusRepositoryReader, IStatusMenusRepositoryWriter
    { }
}

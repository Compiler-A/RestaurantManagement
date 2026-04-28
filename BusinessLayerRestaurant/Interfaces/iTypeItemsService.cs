using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.TypeItems;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface ITypeItemsServiceContainer : IServiceContainer<ITypeItemsRepository>
    { }

    public interface ITypeItemsServiceReader : IServiceReader<TypeItem>
    { }

    public interface ITypeItemsServiceWriter
        : IServiceWriter<TypeItem,DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface ICRUDTypeItemsService : ITypeItemsServiceWriter , ITypeItemsServiceReader
    { }

    public interface ITypeItemsService : ICRUDTypeItemsService { }

}

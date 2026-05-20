using ContractsLayerRestaurant.DTORequest.TypeItems;
using DomainLayer.Entities;
using ContractsLayerRestaurant.Interfaces.Repositories;

namespace ContractsLayerRestaurant.Interfaces.Services
{
    public interface ITypeItemsServiceContainer : IServiceContainer<ITypeItemsRepository>
    { }

    public interface ITypeItemsServiceReader : IServiceReader<TypeItem>
    { }

    public interface ITypeItemsServiceWriter
        : IServiceWriter<TypeItem, DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface ICRUDTypeItemsService : ITypeItemsServiceWriter , ITypeItemsServiceReader
    { }

    public interface ITypeItemsService : ICRUDTypeItemsService { }

}

using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.TypeItems;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface ITypeItemsServiceContainer : IInterfaceBase<ITypeItemsRepository>
    { }

    public interface ITypeItemsServiceReader : IReadableBusinessBase<TypeItem>
    { }

    public interface ITypeItemsServiceWriter
        : IWritableBusinessBase<TypeItem,DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface ITypeItemsServiceReadable
    {
        Task<List<TypeItem>> GetAllTypeItemsAsync(int page);
        Task<TypeItem?> GetTypeItemAsync(int id);
    }

    public interface ITypeItemsServiceWritable
    {
        Task<TypeItem?> AddTypeItemAsync(DTOTypeItemsCRequest Request);
        Task<TypeItem?> UpdateTypeItemAsync(DTOTypeItemsURequest Request);
        Task<bool> DeleteTypeItemAsync(int ID);
    }

    public interface ICRUDTypeItemsService : ITypeItemsServiceWritable , ITypeItemsServiceReadable
    { }

    public interface ITypeItemsService : ICRUDTypeItemsService { }

}

using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.TypeItems;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Interfaces
{

    public interface ITypeItemsRepositoryReader : IReadableDataBase<TypeItem>
    { }

    public interface ITypeItemsRepositoryWriter 
        : IWritableDataBase<TypeItem, DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface ITypeItemsRepositoryWritable
    {
        Task<TypeItem?> AddTypeItemAsync(DTOTypeItemsCRequest Request);
        Task<TypeItem?> UpdateTypeItemAsync(DTOTypeItemsURequest Request);
        Task<bool> DeleteTypeItemAsync(int typeItemID);
    }

    public interface ITypeItemsRepositoryReadable
    {
        Task<TypeItem?> GetTypeItemAsync(int typeItemId);
        Task<List<TypeItem>> GetAllTypeItemsAsync(int page);
    }

    public interface ITypeItemsRepository : ITypeItemsRepositoryReadable, ITypeItemsRepositoryWritable
    {
    }
}

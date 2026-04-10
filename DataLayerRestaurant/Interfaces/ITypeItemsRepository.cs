using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.TypeItems;


namespace DataLayerRestaurant.Interfaces
{

    public interface ITypeItemsRepositoryReader : IReadableDataBase<DTOTypeItems>
    { }

    public interface ITypeItemsRepositoryWriter 
        : IWritableDataBase<DTOTypeItems, DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface ITypeItemsRepositoryWritable
    {
        Task<DTOTypeItems?> AddTypeItemAsync(DTOTypeItemsCRequest Request);
        Task<DTOTypeItems?> UpdateTypeItemAsync(DTOTypeItemsURequest Request);
        Task<bool> DeleteTypeItemAsync(int typeItemID);
    }

    public interface ITypeItemsRepositoryReadable
    {
        Task<DTOTypeItems?> GetTypeItemAsync(int typeItemId);
        Task<List<DTOTypeItems>> GetAllTypeItemsAsync(int page);
    }

    public interface ITypeItemsRepository : ITypeItemsRepositoryReadable, ITypeItemsRepositoryWritable
    {
    }
}

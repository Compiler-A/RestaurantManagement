using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.TypeItems;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface ITypeItemsServiceContainer : IInterfaceBase<IDataTypeItems>
    { }

    public interface ITypeItemsServiceReader : IReadableBusinessBase<DTOTypeItems>
    { }

    public interface ITypeItemsServiceWriter
        : IWritableBusinessBase<DTOTypeItems,DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface ITypeItemsServiceReadable
    {
        Task<List<DTOTypeItems>> GetAllTypeItemsAsync(int page);
        Task<DTOTypeItems?> GetTypeItemAsync(int id);
    }

    public interface ITypeItemsServiceWritable
    {
        Task<DTOTypeItems?> AddTypeItemAsync(DTOTypeItemsCRequest Request);
        Task<DTOTypeItems?> UpdateTypeItemAsync(DTOTypeItemsURequest Request);
        Task<bool> DeleteTypeItemAsync(int ID);
    }

    public interface ICRUDTypeItemsService : ITypeItemsServiceWritable , ITypeItemsServiceReadable
    { }

    public interface IBusinessService : ICRUDTypeItemsService { }

}

using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.TypeItems;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IInterfaceBTypeItems : IInterfaceBase<IDataTypeItems>
    { }

    public interface IReadableBTypeItems : IReadableBusinessBase<DTOTypeItems>
    { }

    public interface IWritableBTypeItems
        : IWritableBusinessBase<DTOTypeItems,DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface IReadableBusinessTypeItems
    {
        Task<List<DTOTypeItems>> GetAllTypeItemsAsync(int page);
        Task<DTOTypeItems?> GetTypeItemAsync(int id);
    }

    public interface IWritableBusinessTypeItems
    {
        Task<DTOTypeItems?> AddTypeItemAsync(DTOTypeItemsCRequest Request);
        Task<DTOTypeItems?> UpdateTypeItemAsync(DTOTypeItemsURequest Request);
        Task<bool> DeleteTypeItemAsync(int ID);
    }

    public interface ICRUDBusinessTypeItems : IWritableBusinessTypeItems , IReadableBusinessTypeItems
    { }

    public interface IBusinessTypeItems : ICRUDBusinessTypeItems { }

}

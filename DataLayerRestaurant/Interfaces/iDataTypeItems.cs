using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public interface IReadableDTypeItems : IReadableDataBase<DTOTypeItems>
    { }

    public interface IWritableDTypeItems 
        : IWritableDataBase<DTOTypeItems, DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }

    public interface IWritableDataTypeItems
    {
        Task<DTOTypeItems?> AddTypeItemAsync(DTOTypeItemsCRequest Request);
        Task<DTOTypeItems?> UpdateTypeItemAsync(DTOTypeItemsURequest Request);
        Task<bool> DeleteTypeItemAsync(int typeItemID);
    }

    public interface IReadableDataTypeItems
    {
        Task<DTOTypeItems?> GetTypeItemAsync(int typeItemId);
        Task<List<DTOTypeItems>> GetAllTypeItemsAsync(int page);
    }

    public interface IDataTypeItems : IReadableDataTypeItems, IWritableDataTypeItems
    {
    }
}

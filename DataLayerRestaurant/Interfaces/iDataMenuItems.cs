using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public interface IReadableDMenuItems : IReadableDataBase<DTOMenuItems>
    {
        Task<List<DTOMenuItems>> GetAllDataAvailablesAsync();
        Task<List<DTOMenuItems>> GetAllDataFiltersAsync(int Page, int StatusMenuID, int TypeItemID);
    }

    public interface IReadableDataMenuItems
    {
        Task<DTOMenuItems?> GetMenuItemAsync(int id);
        Task<List<DTOMenuItems>> GetAllMenuItemsAsync(int page);
        Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(int page, int StatusMenuID, int TypeItemID);
        Task<List<DTOMenuItems>> GetAllMenuItemsAvailablesAsync();
    }


    public interface IWritableDMenuItems : IWritableDataBase<DTOMenuItems, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    {

    }

    public interface IWritableDataMenuItems
    {
        Task<DTOMenuItems?> AddMenuItemAsync(DTOMenuItemsCRequest Request);
        Task<DTOMenuItems?> UpdateMenuItemAsync(DTOMenuItemsURequest Request);
        Task<bool> DeleteMenuItemAsync(int ID);
    }

    public interface IDataMenuItems : IReadableDataMenuItems, IWritableDataMenuItems
    {
    }
}

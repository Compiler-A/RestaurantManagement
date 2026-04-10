using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.MenuItems;

namespace DataLayerRestaurant.Interfaces
{

    public interface IMenuItemsRepositoryReader : IReadableDataBase<DTOMenuItems>
    {
        Task<List<DTOMenuItems>> GetAllDataAvailablesAsync();
        Task<List<DTOMenuItems>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request);
    }

    public interface IMenuItemsRepositoryReadable
    {
        Task<DTOMenuItems?> GetMenuItemAsync(int id);
        Task<List<DTOMenuItems>> GetAllMenuItemsAsync(int page);
        Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<DTOMenuItems>> GetAllMenuItemsAvailablesAsync();
    }


    public interface IMenuItemsRepositoryWriter : IWritableDataBase<DTOMenuItems, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    {

    }

    public interface IMenuItemsRepositoryWritable
    {
        Task<DTOMenuItems?> AddMenuItemAsync(DTOMenuItemsCRequest Request);
        Task<DTOMenuItems?> UpdateMenuItemAsync(DTOMenuItemsURequest Request);
        Task<bool> DeleteMenuItemAsync(int ID);
    }

    public interface IMenuItemsRepository : IMenuItemsRepositoryReadable, IMenuItemsRepositoryWritable
    {
    }
}

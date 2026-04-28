using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.MenuItems;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{

    public interface IMenuItemsRepositoryReader : IReadableDataBase<MenuItem>
    {
        Task<List<MenuItem>> GetAllDataAvailablesAsync();
        Task<List<MenuItem>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request);
    }

    public interface IMenuItemsRepositoryReadable
    {
        Task<MenuItem?> GetMenuItemAsync(int id);
        Task<List<MenuItem>> GetAllMenuItemsAsync(int page);
        Task<List<MenuItem>> GetAllMenuItemsFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<MenuItem>> GetAllMenuItemsAvailablesAsync();
    }


    public interface IMenuItemsRepositoryWriter : IWritableDataBase<MenuItem, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    {

    }

    public interface IMenuItemsRepositoryWritable
    {
        Task<MenuItem?> AddMenuItemAsync(DTOMenuItemsCRequest Request);
        Task<MenuItem?> UpdateMenuItemAsync(DTOMenuItemsURequest Request);
        Task<bool> DeleteMenuItemAsync(int ID);
    }

    public interface IMenuItemsRepository : IMenuItemsRepositoryReadable, IMenuItemsRepositoryWritable
    {
    }
}

using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.MenuItems;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface IMenuItemsServiceContainer : IServiceContainer<IMenuItemsRepository>
    {
        ITypeItemsService IBusinessTypeItem { get; set; }
        IStatusMenusService IBusinessStatusMenu { get; set; }
    }

    public interface IMenuItemsServiceReader : IServiceReader<MenuItem>
    {
        Task<List<MenuItem>> GetAllFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<MenuItem>> GetAllAvailablesAsync();
    }

    public interface IMenuItemsServiceWriter 
        : IServiceWriter<MenuItem, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    { }
    public interface IMenuItemsServiceComposition
    {
        Task LoadDataAsync(MenuItem item);
    }

    public interface IMenuItemsServiceContainers
    {
        ITypeItemsService ITypeItem { get; set; }
        IStatusMenusService IStatusMenu { get; set; }
    }


    public interface IMenuItemsServiceReadable
    {
        Task<List<MenuItem>> GetAllMenuItemsAsync(int page);
        Task<MenuItem?> GetMenuItemAsync(int id);
        Task<List<MenuItem>> GetAllMenuItemsFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<MenuItem>> GetAllMenuItemsAvailablesAsync();
    }

    public interface IMenuItemsServiceWritable
    {
        Task<MenuItem?> AddMenuItemAsync(DTOMenuItemsCRequest Request);
        Task<MenuItem?> UpdateMenuItemAsync(DTOMenuItemsURequest Request);
        Task<bool> DeleteMenuItemAsync(int ID);
    }

    public interface ICRUDMenuItemsService : IMenuItemsServiceReadable , IMenuItemsServiceWritable
    { }

    public interface IMenuItemsService : ICRUDMenuItemsService, IMenuItemsServiceContainers { }

}

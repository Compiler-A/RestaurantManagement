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
    public interface IMenuItemsServiceComposition
    {
        Task LoadDataAsync(MenuItem item);
    }

    public interface IMenuItemsServiceContainers
    {
        ITypeItemsService ITypeItem { get; set; }
        IStatusMenusService IStatusMenu { get; set; }
    }

    public interface IMenuItemsServiceReader : IServiceReader<MenuItem>
    {
        Task<List<MenuItem>> GetAllFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<MenuItem>> GetAllAvailablesAsync();
    }

    public interface IMenuItemsServiceWriter 
        : IServiceWriter<MenuItem, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    { }

    public interface ICRUDMenuItemsService : IMenuItemsServiceReader , IMenuItemsServiceWriter
    { }

    public interface IMenuItemsService : ICRUDMenuItemsService, IMenuItemsServiceContainers { }

}

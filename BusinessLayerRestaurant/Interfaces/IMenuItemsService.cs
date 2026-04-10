using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.MenuItems;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface IMenuItemsServiceContainer : IInterfaceBase<IDataMenuItems>
    {
        IBusinessService IBusinessTypeItem { get; set; }
        IStatusMenusService IBusinessStatusMenu { get; set; }
    }

    public interface IMenuItemsServiceReader : IReadableBusinessBase<DTOMenuItems>
    {
        Task<List<DTOMenuItems>> GetAllFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<DTOMenuItems>> GetAllAvailablesAsync();
    }

    public interface IMenuItemsServiceWriter 
        : IWritableBusinessBase<DTOMenuItems, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    { }
    public interface IMenuItemsServiceComposition
    {
        Task LoadDataAsync(DTOMenuItems item);
    }

    public interface IMenuItemsServiceContainers
    {
        IBusinessService ITypeItem { get; set; }
        IStatusMenusService IStatusMenu { get; set; }
    }


    public interface IMenuItemsServiceReadable
    {
        Task<List<DTOMenuItems>> GetAllMenuItemsAsync(int page);
        Task<DTOMenuItems?> GetMenuItemAsync(int id);
        Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<DTOMenuItems>> GetAllMenuItemsAvailablesAsync();
    }

    public interface IMenuItemsServiceWritable
    {
        Task<DTOMenuItems?> AddMenuItemAsync(DTOMenuItemsCRequest Request);
        Task<DTOMenuItems?> UpdateMenuItemAsync(DTOMenuItemsURequest Request);
        Task<bool> DeleteMenuItemAsync(int ID);
    }

    public interface ICRUDMenuItemsService : IMenuItemsServiceReadable , IMenuItemsServiceWritable
    { }

    public interface IMenuItemsService : ICRUDMenuItemsService, IMenuItemsServiceContainers { }

}

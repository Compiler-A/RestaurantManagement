using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.MenuItems;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface IInterfaceBMenuItems : IInterfaceBase<IDataMenuItems>
    {
        IBusinessTypeItems IBusinessTypeItem { get; set; }
        IBusinessStatusMenus IBusinessStatusMenu { get; set; }
    }

    public interface IReadableBMenuItems : IReadableBusinessBase<DTOMenuItems>
    {
        Task<List<DTOMenuItems>> GetAllFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<DTOMenuItems>> GetAllAvailablesAsync();
    }

    public interface IWritableBMenuItems 
        : IWritableBusinessBase<DTOMenuItems, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    { }
    public interface ICompositionBMenuItems
    {
        Task LoadDataAsync(DTOMenuItems item);
    }

    public interface IInterfaceBusinessMenuItems
    {
        IBusinessTypeItems ITypeItem { get; set; }
        IBusinessStatusMenus IStatusMenu { get; set; }
    }


    public interface IReadableBusinessMenuItems
    {
        Task<List<DTOMenuItems>> GetAllMenuItemsAsync(int page);
        Task<DTOMenuItems?> GetMenuItemAsync(int id);
        Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(DTOMenuItemsFilterRequest Request);
        Task<List<DTOMenuItems>> GetAllMenuItemsAvailablesAsync();
    }

    public interface IWritableBusinessMenuItems
    {
        Task<DTOMenuItems?> AddMenuItemAsync(DTOMenuItemsCRequest Request);
        Task<DTOMenuItems?> UpdateMenuItemAsync(DTOMenuItemsURequest Request);
        Task<bool> DeleteMenuItemAsync(int ID);
    }

    public interface ICRUDBusinessMenuItems : IReadableBusinessMenuItems , IWritableBusinessMenuItems
    { }

    public interface IBusinessMenuItems : ICRUDBusinessMenuItems, IInterfaceBusinessMenuItems { }

}

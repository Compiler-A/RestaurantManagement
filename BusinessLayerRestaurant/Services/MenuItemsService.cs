using ContractsLayerRestaurant.DTORequest.MenuItems;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{


    public class MenuItemsService : IMenuItemsService
    {
        IMenuItemsServiceContainer _Interface;
        IMenuItemsServiceReader _IRead;
        IMenuItemsServiceWriter _IWrite;

        public MenuItemsService(IMenuItemsServiceContainer Interface, IMenuItemsServiceReader IRead, IMenuItemsServiceWriter IWrite)
        {
            _Interface = Interface;
            _IRead = IRead;
            _IWrite = IWrite;
        }

        public IStatusMenusService IStatusMenu
        {
            get => _Interface.IBusinessStatusMenu;
            set => _Interface.IBusinessStatusMenu = value;
        }

        public ITypeItemsService ITypeItem
        {
            get => _Interface.IBusinessTypeItem;
            set => _Interface.IBusinessTypeItem = value;
        }

        public async Task<MenuItem?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }
        public async Task<List<MenuItem>> GetAllAsync(int Page)
        {
            return await _IRead.GetAllAsync(Page);
        }

        public async Task<List<MenuItem>> GetAllAvailablesAsync()
        {
            return await _IRead.GetAllAvailablesAsync();
        }

        public async Task<List<MenuItem>> GetAllFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            return await _IRead.GetAllFiltersAsync(Request);
        }

        public async Task<MenuItem?> CreateAsync(DTOMenuItemsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }

        public async Task<MenuItem?> UpdateAsync(DTOMenuItemsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayerRestaurant;

namespace BusinessLayerRestaurant
{

    public interface IReadableMenuItemsBusiness
    {
        Task<List<DTOMenuItem>> GetAllMenuItemsAsync(int page);
        Task<DTOMenuItem?> GetMenuItemByIdAsync(int id);
        Task<List<DTOMenuItem>> GetFilterAllMenuItemsAsync(int page, int StatusMenuID, int TypeItemID);
        Task<List<DTOMenuItem>> GetAllMenuItemsAvailables();
    }
    
    public interface IWritableMenuItemsBusiness
    {
        Task<bool> SaveAsync();
        Task<bool> DeleteAsync();
    }

    public interface IBusinessMenuItems : IReadableMenuItemsBusiness, IWritableMenuItemsBusiness { }

    public class clsBusinessMenuItem : IBusinessMenuItems
    {
        public enum enMode
        {
            Add = 0,
            Update
        }

        private readonly IDataMenuItems _menuItemRepo;
        private readonly IDataTypeItems _typeItemRepo;
        private readonly IDataStatusMenus _statusMenuRepo;

        private DTOMenuItem? _menuItem;
        public DTOMenuItem? MenuItem
        {
            get { return _menuItem; }
            set { _menuItem = value; }
        }

        public enMode Mode { get; private set; } = enMode.Add;

        public clsBusinessMenuItem(DTOMenuItem? menuItem =null , enMode mode = enMode.Add)
        {
            _menuItem = menuItem;
            _menuItemRepo = new clsDataMenuItems();
            _typeItemRepo = new clsDataTypeItems();
            _statusMenuRepo = new clsDataStatusMenus();
            Mode = mode;
        }

        private async Task LoadRelatedObjectsAsync()
        {
            MenuItem!.TypeItems = await _typeItemRepo.GetTypeItemById(MenuItem.TypeItemID);
            MenuItem.StatusMenus = await _statusMenuRepo.GetStatusMenusByID(MenuItem.StatusMenuID);
        }

        private async Task<bool> AddMenuItemAsync()
        {
            MenuItem!.MenuItemID = await _menuItemRepo.AddMenuItem(MenuItem);
            if (MenuItem.MenuItemID != -1)
            {
                Mode = enMode.Update; 
                return true;
            }
            return false;
        }

        private async Task<bool> UpdateMenuItemAsync()
        {
            return await _menuItemRepo.UpdateMenuItem(MenuItem!);
        }

        public async Task<bool> SaveAsync()
        {
            bool result = false;
            switch (Mode)
            {
                case enMode.Add:
                    result = await AddMenuItemAsync();
                    break;
                case enMode.Update:
                    result = await UpdateMenuItemAsync();
                    break;
            }

            if (result)
            {
                await LoadRelatedObjectsAsync(); 
            }

            return result;
        }

        public async Task<bool> DeleteAsync()
        {
            if (MenuItem == null) return false;
            return await _menuItemRepo.DeleteMenuItem(MenuItem.MenuItemID);
        }

        public static async Task<clsBusinessMenuItem> CreateBusinessMenuItemAsync(DTOMenuItem menuItem, enMode mode = enMode.Add)
        {
            var businessItem = new clsBusinessMenuItem(menuItem, mode);
            await businessItem.LoadRelatedObjectsAsync();
            return businessItem;
        }

        private async Task LoadRelatedObjectsForListAsync(List<DTOMenuItem> menuItems)
        {
            foreach (var item in menuItems)
            {
                item.TypeItems = await _typeItemRepo.GetTypeItemById(item.TypeItemID);
                item.StatusMenus = await _statusMenuRepo.GetStatusMenusByID(item.StatusMenuID);
            }
        }

        public  async Task<List<DTOMenuItem>> GetAllMenuItemsAsync(int page)
        {
            var list = await _menuItemRepo.GetAllMenuItems(page);
 
            await LoadRelatedObjectsForListAsync(list);

            return list;
        }

        public async Task<List<DTOMenuItem>> GetFilterAllMenuItemsAsync(int page, int StatusMenuID, int TypeItemID)
        {
            var list = await _menuItemRepo.GetFilterAllMenuItems(page, StatusMenuID, TypeItemID);
            await LoadRelatedObjectsForListAsync(list);
            return list;
        }

        public async Task<List<DTOMenuItem>> GetAllMenuItemsAvailables()
        {
            var list = await _menuItemRepo.GetAllMenuItemsAvailables();
            await LoadRelatedObjectsForListAsync(list);
            return list;
        }

        public async Task<DTOMenuItem?> GetMenuItemByIdAsync(int id)
        {
            var dto = await _menuItemRepo.GetMenuItemByID(id);
            if (dto == null) return null;

            dto.TypeItems = await _typeItemRepo.GetTypeItemById(dto.TypeItemID);
            dto.StatusMenus = await _statusMenuRepo.GetStatusMenusByID(dto.StatusMenuID);
            return dto;
        }
    }
}

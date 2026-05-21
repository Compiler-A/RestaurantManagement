#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using ContractsLayerRestaurant.DTORequest.MenuItems;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Operations
{

    public class MenuItemsContainer : IMenuItemsServiceContainer
    {
        private IMenuItemsRepository _IData;
        public IMenuItemsRepository IData
        {
            get => _IData;
            set => _IData = value;
        }

        private ITypeItemsService _IBusinessTypeItem;
        public ITypeItemsService IBusinessTypeItem
        {
            get => _IBusinessTypeItem;
            set => _IBusinessTypeItem = value;
        }
        private IStatusMenusService _IBusinessStatusMenu;
        public IStatusMenusService IBusinessStatusMenu
        {
            get => _IBusinessStatusMenu;
            set => _IBusinessStatusMenu = value;
        }

        public MenuItemsContainer(IMenuItemsRepository iData, ITypeItemsService iTypeItem, IStatusMenusService iStatusMenu)
        {
            _IData = iData;
            _IBusinessTypeItem = iTypeItem;
            _IBusinessStatusMenu = iStatusMenu;
        }
    }


    public class MenuItemsReader : IMenuItemsServiceReader
    {
        IMenuItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public MenuItemsReader(IMenuItemsServiceContainer Interface,IMyLogger Logger)
        { 
            _Interface = Interface;
            _Logger = Logger;
        }

        public async Task<MenuItem?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetDataAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            _Logger.EventLogs($"MenuItem Found, Name: {dto.ItemName}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<List<MenuItem>> GetAllAsync(int Page)
        {
            var list = await _Interface.IData.GetAllDataAsync(Page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            _Logger.EventLogs($"MenuItems Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }

        public async Task<List<MenuItem>> GetAllAvailablesAsync()
        {
            var list = await _Interface.IData.GetAllDataAvailablesAsync();
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            _Logger.EventLogs($"MenuItems Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }

        public async Task<List<MenuItem>> GetAllFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            var list = await _Interface.IData.GetAllDataFiltersAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }


            _Logger.EventLogs($"MenuItems Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }
    }

    public class MenuItemsWriter : IMenuItemsServiceWriter
    {
        IMenuItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public MenuItemsWriter(IMenuItemsServiceContainer Interface, IMyLogger Logger)
        {
            _Interface = Interface;
            _Logger = Logger;
        }

        public async Task<MenuItem?> CreateAsync(DTOMenuItemsCRequest Request)
        {
            var dto = await _Interface.IData.CreateDataAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"MenuItem Created, Name: {dto.ItemName}", EventLogEntryType.Information);

            return dto;
        }

        public async Task<MenuItem?> UpdateAsync(DTOMenuItemsURequest Request)
        {
            var dto = await _Interface.IData.UpdateDataAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"MenuItem Updated, Name: {dto.ItemName}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interface.IData.DeleteDataAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"MenuItem Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }
    }
}

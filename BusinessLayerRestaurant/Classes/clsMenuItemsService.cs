#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.MenuItems;



namespace BusinessLayerRestaurant.Classes
{

    public class clsMenuItemsContainer : IMenuItemsServiceContainer
    {
        private IDataMenuItems _IData;
        public IDataMenuItems IData
        {
            get => _IData;
            set => _IData = value;
        }

        private IBusinessService _IBusinessTypeItem;
        public IBusinessService IBusinessTypeItem
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

        public clsMenuItemsContainer(IDataMenuItems iData, IBusinessService iTypeItem, IStatusMenusService iStatusMenu)
        {
            _IData = iData;
            _IBusinessTypeItem = iTypeItem;
            _IBusinessStatusMenu = iStatusMenu;
        }
    }

    public class clsTypeItemLoader : IMenuItemsServiceComposition
    {
        private IBusinessService _BusinessTypeItem;
        public clsTypeItemLoader(IBusinessService BusinessTypeItem)
        {
            _BusinessTypeItem = BusinessTypeItem;
        }

        public async Task LoadDataAsync(DTOMenuItems item)
        {
            item.TypeItems = await _BusinessTypeItem.GetTypeItemAsync(item.TypeItemID);
        }
    }

    public class clsStatusMenuLoader : IMenuItemsServiceComposition
    {
        private IStatusMenusService _Business;
        public clsStatusMenuLoader(IStatusMenusService Business)
        {
            _Business = Business;
        }

        public async Task LoadDataAsync(DTOMenuItems item)
        {
            item.StatusMenus = await _Business.GetStatusMenuAsync(item.StatusMenuID);
        }
    }

    public class clsCompositionMenuItemsLoader : IMenuItemsServiceComposition
    {
        IEnumerable<IMenuItemsServiceComposition> _Interface;

        public clsCompositionMenuItemsLoader(IEnumerable<IMenuItemsServiceComposition> Interface)
        {
            _Interface = Interface;
        }

        public async Task LoadDataAsync(DTOMenuItems Item)
        {
            foreach (var inter in _Interface)
            {
                await inter.LoadDataAsync(Item);
            }
        }

    }

    public class clsMenuItemsReader : clsCompositionMenuItemsLoader ,IMenuItemsServiceReader
    {
        IMenuItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsMenuItemsReader(IMenuItemsServiceContainer Interface,IMyLogger Logger, IEnumerable<IMenuItemsServiceComposition> loader)
            : base(loader)
        { 
            _Interface = Interface;
            _Logger = Logger;
        }

        public async Task<DTOMenuItems?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetMenuItemAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            _Logger.EventLogs($"MenuItem Found, Name: {dto.Name}", EventLogEntryType.Information);
            await LoadDataAsync(dto);
            return dto;
        }

        public async Task<List<DTOMenuItems>> GetAllAsync(int Page)
        {
            var list = await _Interface.IData.GetAllMenuItemsAsync(Page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"MenuItems Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }

        public async Task<List<DTOMenuItems>> GetAllAvailablesAsync()
        {
            var list = await _Interface.IData.GetAllMenuItemsAvailablesAsync();
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"MenuItems Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }

        public async Task<List<DTOMenuItems>> GetAllFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            var list = await _Interface.IData.GetAllMenuItemsFiltersAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"MenuItems Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }
    }

    public class clsMenuItemsWriter : clsCompositionMenuItemsLoader , IMenuItemsServiceWriter
    {
        IMenuItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsMenuItemsWriter(IMenuItemsServiceContainer Interface,IMyLogger Logger, IEnumerable<IMenuItemsServiceComposition> loader)
            : base(loader)
        {
            _Interface = Interface;
            _Logger = Logger;
        }

        public async Task<DTOMenuItems?> CreateAsync(DTOMenuItemsCRequest Request)
        {
            var dto  = await _Interface.IData.AddMenuItemAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            await LoadDataAsync(dto);
            _Logger.EventLogs($"MenuItem Created, Name: {dto.Name}", EventLogEntryType.Information);

            return dto;
        }
        
        public async Task<DTOMenuItems?> UpdateAsync(DTOMenuItemsURequest Request)
        {
            var dto = await _Interface.IData.UpdateMenuItemAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            await LoadDataAsync(dto);
            _Logger.EventLogs($"MenuItem Updated, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interface.IData.DeleteMenuItemAsync(ID);
            if (!isDeleted)
            {
                    throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"MenuItem Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }
    }


    public class clsMenuItemService : IMenuItemsService
    {
        IMenuItemsServiceContainer _Interface;
        IMenuItemsServiceReader _IRead;
        IMenuItemsServiceWriter _IWrite;

        public clsMenuItemService( IMenuItemsServiceContainer Interface, IMenuItemsServiceReader IRead, IMenuItemsServiceWriter IWrite)
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

        public IBusinessService ITypeItem
        {
            get => _Interface.IBusinessTypeItem;
            set => _Interface.IBusinessTypeItem = value;
        }

        public async Task<DTOMenuItems?> GetMenuItemAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }
        public async Task<List<DTOMenuItems>> GetAllMenuItemsAsync(int Page)
        {
            return await _IRead.GetAllAsync(Page);
        }

        public async Task<List<DTOMenuItems>> GetAllMenuItemsAvailablesAsync()
        {
            return await _IRead.GetAllAvailablesAsync();
        }

        public async Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            return await _IRead.GetAllFiltersAsync(Request);
        }

        public async Task<DTOMenuItems?> AddMenuItemAsync(DTOMenuItemsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }

        public async Task<DTOMenuItems?> UpdateMenuItemAsync(DTOMenuItemsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteMenuItemAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID); 
        }

    }
}

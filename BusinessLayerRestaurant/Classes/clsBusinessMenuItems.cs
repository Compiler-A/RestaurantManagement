#pragma warning disable CA1416 // Validate platform compatibility
using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace BusinessLayerRestaurant
{

    public class clsMenuItemsDtoContainer : IDTOBMenuItems
    {
        private DTOMenuItemsCRequest? _CreateRequest;
        public DTOMenuItemsCRequest? CreateRequest
        {
            get => _CreateRequest;
            set => _CreateRequest = value;
        }

        private DTOMenuItemsURequest? _UpdateRequest;
        public DTOMenuItemsURequest? UpdateRequest
        {
            get => _UpdateRequest;
            set => _UpdateRequest = value;
        }
    }


    public class clsMenuItemsRepositoryBridge : IInterfaceBMenuItems
    {
        private IDataMenuItems _IData;
        public IDataMenuItems IData
        {
            get => _IData;
            set => _IData = value;
        }

        private IBusinessTypeItems _IBusinessTypeItem;
        public IBusinessTypeItems IBusinessTypeItem
        {
            get => _IBusinessTypeItem;
            set => _IBusinessTypeItem = value;
        }
        private IBusinessStatusMenus _IBusinessStatusMenu;
        public IBusinessStatusMenus IBusinessStatusMenu
        {
            get => _IBusinessStatusMenu;
            set => _IBusinessStatusMenu = value;
        }

        public clsMenuItemsRepositoryBridge(IDataMenuItems iData, IBusinessTypeItems iTypeItem, IBusinessStatusMenus iStatusMenu)
        {
            _IData = iData;
            _IBusinessTypeItem = iTypeItem;
            _IBusinessStatusMenu = iStatusMenu;
        }
    }

    public class clsTypeItemLoader : ICompositionBMenuItems
    {
        private IBusinessTypeItems _BusinessTypeItem;
        public clsTypeItemLoader(IBusinessTypeItems BusinessTypeItem)
        {
            _BusinessTypeItem = BusinessTypeItem;
        }

        public async Task LoadDataAsync(DTOMenuItems item)
        {
            item.TypeItems = await _BusinessTypeItem.GetTypeItemAsync(item.TypeItemID);
        }
    }

    public class clsStatusMenuLoader : ICompositionBMenuItems
    {
        private IBusinessStatusMenus _Business;
        public clsStatusMenuLoader(IBusinessStatusMenus Business)
        {
            _Business = Business;
        }

        public async Task LoadDataAsync(DTOMenuItems item)
        {
            item.StatusMenus = await _Business.GetStatusMenuAsync(item.StatusMenuID);
        }
    }

    public class clsCompositionMenuItemsLoader : ICompositionBMenuItems
    {
        IEnumerable<ICompositionBMenuItems> _Interface;

        public clsCompositionMenuItemsLoader(IEnumerable<ICompositionBMenuItems> Interface)
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

    public class clsMenuItemsReader : clsCompositionMenuItemsLoader ,IReadableBMenuItems
    {
        IInterfaceBMenuItems _Interface;
        private IMyLogger _Logger;
        public clsMenuItemsReader(IInterfaceBMenuItems Interface,IMyLogger Logger, IEnumerable<ICompositionBMenuItems> loader)
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

    public class clsMenuItemsWriter : clsCompositionMenuItemsLoader , IWritableBMenuItems
    {
        IInterfaceBMenuItems _Interface;
        private IMyLogger _Logger;
        public clsMenuItemsWriter(IInterfaceBMenuItems Interface,IMyLogger Logger, IEnumerable<ICompositionBMenuItems> loader)
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


    public class clsBusinessMenuItem : IBusinessMenuItems
    {
        IDTOBMenuItems _IDTO;
        IInterfaceBMenuItems _Interface;
        IReadableBMenuItems _IRead;
        IWritableBMenuItems _IWrite;

        public clsBusinessMenuItem(IDTOBMenuItems DTO, IInterfaceBMenuItems Interface, IReadableBMenuItems IRead, IWritableBMenuItems IWrite)
        {
            _IDTO = DTO;
            _Interface = Interface;
            _IRead = IRead;
            _IWrite = IWrite;
        }

        public DTOMenuItemsCRequest? CreateRequest
        {
            get => _IDTO.CreateRequest;
            set => _IDTO.CreateRequest = value;
        }

        public DTOMenuItemsURequest? UpdateRequest
        {
            get => _IDTO.UpdateRequest;
            set => _IDTO.UpdateRequest = value;
        }

        public IBusinessStatusMenus IStatusMenu
        {
            get => _Interface.IBusinessStatusMenu;
            set => _Interface.IBusinessStatusMenu = value;
        }

        public IBusinessTypeItems ITypeItem
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

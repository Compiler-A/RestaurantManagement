using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayerRestaurant;


namespace BusinessLayerRestaurant
{

    public class clsDTOBMenuItems : IDTOBMenuItems
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


    public class clsInterfaceBMenuItems : IInterfaceBMenuItems
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

        public clsInterfaceBMenuItems(IDataMenuItems iData, IBusinessTypeItems iTypeItem, IBusinessStatusMenus iStatusMenu)
        {
            _IData = iData;
            _IBusinessTypeItem = iTypeItem;
            _IBusinessStatusMenu = iStatusMenu;
        }
    }

    public class clsTypeItemLoaderByMenuItems : ICompositionBMenuItems
    {
        private IBusinessTypeItems _BusinessTypeItem;
        public clsTypeItemLoaderByMenuItems(IBusinessTypeItems BusinessTypeItem)
        {
            _BusinessTypeItem = BusinessTypeItem;
        }

        public async Task LoadDataAsync(DTOMenuItems item)
        {
            item.TypeItems = await _BusinessTypeItem.GetTypeItemAsync(item.TypeItemID);
        }
    }

    public class clsStatusMenuLoaderByMenuItems : ICompositionBMenuItems
    {
        private IBusinessStatusMenus _Business;
        public clsStatusMenuLoaderByMenuItems(IBusinessStatusMenus Business)
        {
            _Business = Business;
        }

        public async Task LoadDataAsync(DTOMenuItems item)
        {
            item.StatusMenus = await _Business.GetStatusMenuAsync(item.StatusMenuID);
        }
    }

    public class clsCompositionBMenuItems : ICompositionBMenuItems
    {
        IEnumerable<ICompositionBMenuItems> _Interface;

        public clsCompositionBMenuItems(IEnumerable<ICompositionBMenuItems> Interface)
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

    public class clsReadableBMenuItems : clsCompositionBMenuItems ,IReadableBMenuItems
    {
        IInterfaceBMenuItems _Interface;

        public clsReadableBMenuItems(IInterfaceBMenuItems Interface, IEnumerable<ICompositionBMenuItems> loader)
            : base(loader)
        { 
            _Interface = Interface;
        }

        public async Task<DTOMenuItems?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetMenuItemAsync(ID);
            if (dto == null)
            {
                return null;
            }

            await LoadDataAsync(dto);
            return dto;
        }

        public async Task<List<DTOMenuItems>> GetAllAsync(int Page)
        {
            var list = await _Interface.IData.GetAllMenuItemsAsync(Page);
            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }
            return list;
        }

        public async Task<List<DTOMenuItems>> GetAllAvailablesAsync()
        {
            var list = await _Interface.IData.GetAllMenuItemsAvailablesAsync();
            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }

            return list;
        }

        public async Task<List<DTOMenuItems>> GetAllFiltersAsync(int Page, int StatusMenuID, int TypeItemID)
        {
            var list = await _Interface.IData.GetAllMenuItemsFiltersAsync(Page, StatusMenuID, TypeItemID);
            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }

            return list;
        }
    }

    public class clsWritableBMenuItem : clsCompositionBMenuItems , IWritableBMenuItems
    {
        IInterfaceBMenuItems _Interface;

        public clsWritableBMenuItem(IInterfaceBMenuItems Interface, IEnumerable<ICompositionBMenuItems> loader)
            : base(loader)
        {
            _Interface = Interface;
        }

        public async Task<DTOMenuItems?> CreateAsync(DTOMenuItemsCRequest Request)
        {
            var dto  = await _Interface.IData.AddMenuItemAsync(Request);
            if (dto == null)
            {
                return null;
            }
            await LoadDataAsync(dto);

            return dto;
        }

        public async Task<DTOMenuItems?> UpdateAsync(DTOMenuItemsURequest Request)
        {
            var dto = await _Interface.IData.UpdateMenuItemAsync(Request);
            if (dto == null)
            {
                return null;
            }
            await LoadDataAsync(dto);

            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interface.IData.DeleteMenuItemAsync(ID);
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

        public async Task<List<DTOMenuItems>> GetAllMenuItemsFiltersAsync(int Page, int StatusMenuID, int TypeItemID)
        {
            return await _IRead.GetAllFiltersAsync(Page, StatusMenuID, TypeItemID);
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

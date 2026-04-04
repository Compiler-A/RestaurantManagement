using DataLayerRestaurant;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace BusinessLayerRestaurant
{

    public class clsTypeItemsDtoContainer : IDTOBTypeItems
    {
        private DTOTypeItemsCRequest? _CRequest;
        private DTOTypeItemsURequest? _URequest;

        public DTOTypeItemsCRequest? CreateRequest
        {
            get => _CRequest;
            set => _CRequest = value;
        }

        public DTOTypeItemsURequest? UpdateRequest
        {
            get => _URequest;
            set => _URequest = value;
        }
    }

    public class clsTypeItemsRepositoryBridge : IInterfaceBTypeItems
    {
        private IDataTypeItems _IDataTypeItem;
        public IDataTypeItems IData
        {
            get => _IDataTypeItem;
            set => _IDataTypeItem = value;
        }

        public clsTypeItemsRepositoryBridge(IDataTypeItems Data)
        {
            _IDataTypeItem = Data;
        }
    }

    public class clsTypeItemsReader : IReadableBTypeItems
    {
        private IInterfaceBTypeItems _Interface;

        public clsTypeItemsReader(IInterfaceBTypeItems Interface)
        {
            _Interface = Interface;
        }
        public async Task<DTOTypeItems?> GetAsync(int ID)
        {
            var result  = await _Interface.IData.GetTypeItemAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return result;
        }
        public async Task<List<DTOTypeItems>> GetAllAsync(int page)
        {
            var result = await _Interface.IData.GetAllTypeItemsAsync(page);
            if (result == null || result.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            return result;
        }
    }

    public class clsTypeItemsWriter : IWritableBTypeItems
    {
        private IInterfaceBTypeItems _Interface;
        public clsTypeItemsWriter(IInterfaceBTypeItems @interface)
        {
            _Interface = @interface;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interface.IData.DeleteTypeItemAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted");
            }
            return isDeleted;
        }
        public async Task<DTOTypeItems?> UpdateAsync(DTOTypeItemsURequest Request)
        { 
            var result = await _Interface.IData.UpdateTypeItemAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            return result;
        }

        public async Task<DTOTypeItems?> CreateAsync(DTOTypeItemsCRequest Request)
        {
            var result = await _Interface.IData.AddTypeItemAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            return result;
        }
    }



    public class clsBusinessTypeItems : IBusinessTypeItems
    {
        IDTOBTypeItems _IDTO;
        IWritableBTypeItems _IWrite;
        IReadableBTypeItems _IRead;

        public clsBusinessTypeItems(IDTOBTypeItems dto, IWritableBTypeItems write,  IReadableBTypeItems read)
        {
            _IDTO = dto;
            _IWrite = write;
            _IRead = read;
        }

        public DTOTypeItemsCRequest? CreateRequest
        {
            get => _IDTO.CreateRequest;
            set => _IDTO.CreateRequest = value;
        }
        public DTOTypeItemsURequest? UpdateRequest
        {
            get => _IDTO.UpdateRequest;
            set => _IDTO.UpdateRequest = value;
        }

        public async Task<DTOTypeItems?> GetTypeItemAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }
        public async Task<List<DTOTypeItems>> GetAllTypeItemsAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<DTOTypeItems?> AddTypeItemAsync(DTOTypeItemsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOTypeItems?> UpdateTypeItemAsync(DTOTypeItemsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteTypeItemAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

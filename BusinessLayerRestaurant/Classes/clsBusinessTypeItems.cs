using DataLayerRestaurant;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace BusinessLayerRestaurant
{

    public class clsDTOBTypeItems : IDTOBTypeItems
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

    public class clsInterfaceBTypeItems : IInterfaceBTypeItems
    {
        private IDataTypeItems _IDataTypeItem;
        public IDataTypeItems IData
        {
            get => _IDataTypeItem;
            set => _IDataTypeItem = value;
        }

        public clsInterfaceBTypeItems(IDataTypeItems Data)
        {
            _IDataTypeItem = Data;
        }
    }

    public class clsReadableBTypeItems : IReadableBTypeItems
    {
        private IInterfaceBTypeItems _Interface;

        public clsReadableBTypeItems(IInterfaceBTypeItems Interface)
        {
            _Interface = Interface;
        }
        public async Task<DTOTypeItems?> GetAsync(int ID)
        {
            return await _Interface.IData.GetTypeItemAsync(ID);
        }
        public async Task<List<DTOTypeItems>> GetAllAsync(int page)
        {
            return await _Interface.IData.GetAllTypeItemsAsync(page);
        }
    }

    public class clsWritableBTypeItems : IWritableBTypeItems
    {
        private IInterfaceBTypeItems _Interface;
        public clsWritableBTypeItems(IInterfaceBTypeItems @interface)
        {
            _Interface = @interface;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interface.IData.DeleteTypeItemAsync(ID);
        }
        public async Task<DTOTypeItems?> UpdateAsync(DTOTypeItemsURequest Request)
        { 
            return await _Interface.IData.UpdateTypeItemAsync(Request);
        }
        public async Task<DTOTypeItems?> CreateAsync(DTOTypeItemsCRequest Request)
        {
            return await _Interface.IData.AddTypeItemAsync(Request);
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

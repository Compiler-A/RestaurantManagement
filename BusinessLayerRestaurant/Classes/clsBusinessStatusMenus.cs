using DataLayerRestaurant;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public class clsDTOBStatusMenus : IDTOBStatusMenus
    {
        private DTOStatusMenusCRequest? _CRequest;
        public DTOStatusMenusCRequest? CreateRequest
        {
            get => _CRequest;
            set => _CRequest = value;
        }
        private DTOStatusMenusURequest? _URequest;
        public DTOStatusMenusURequest? UpdateRequest
        {
            get => _URequest;
            set => _URequest = value;
        }
    }

    public class clsInterfaceBStatusMenus : IInterfaceBStatusMenus
    {
        private IDataStatusMenus _IData;
        public IDataStatusMenus IData
        {
            get => _IData;
            set => _IData = value;
        }

        public clsInterfaceBStatusMenus(IDataStatusMenus IData)
        {
            _IData = IData;
        }
    }

    public class clsReadableBStatusMenus : IReadableBStatusMenus
    {
        private IInterfaceBStatusMenus _Interface;
        public clsReadableBStatusMenus(IInterfaceBStatusMenus @interface)
        {
            _Interface = @interface;
        }

        public async Task<DTOStatusMenus?> GetAsync(int ID)
        {
            return await _Interface.IData.GetStatusMenuAsync(ID);
        }
        public async Task<List<DTOStatusMenus>> GetAllAsync(int page)
        {
            return await _Interface.IData.GetAllStatusMenusAsync(page);
        }
    }

    public class clsWritableBStatusMenus : IWritableBStatusMenus
    {
        private IInterfaceBStatusMenus _Interface;
        public clsWritableBStatusMenus(IInterfaceBStatusMenus @interface)
        {
            _Interface = @interface;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interface.IData.DeleteStatusMenuAsync(ID);
        }
        public async Task<DTOStatusMenus?> UpdateAsync(DTOStatusMenusURequest Request)
        {
            return await _Interface.IData.UpdateStatusMenuAsync(Request);
        }
        public async Task<DTOStatusMenus?> CreateAsync(DTOStatusMenusCRequest Request)
        {
            return await _Interface.IData.AddStatusMenuAsync(Request);
        }
    }

    
    public class clsBusinessStatusMenus : IBusinessStatusMenus
    {
        IDTOBStatusMenus _IDTO;
        IReadableBStatusMenus _IRead;
        IWritableBStatusMenus _IWrite;

        public clsBusinessStatusMenus(IDTOBStatusMenus DTO, IWritableBStatusMenus Write, IReadableBStatusMenus Read)
        {
            _IDTO = DTO;
            _IRead = Read;
            _IWrite = Write;
        }

        public DTOStatusMenusCRequest? CreateRequest
        {
            get => _IDTO.CreateRequest;
            set => _IDTO.CreateRequest = value;
        }

        public DTOStatusMenusURequest? UpdateRequest
        {
            get => _IDTO.UpdateRequest;
            set => _IDTO.UpdateRequest = value;
        }

        public async Task<List<DTOStatusMenus>> GetAllStatusMenusAsync(int Page)
        {
            return await _IRead.GetAllAsync(Page);
        }
        public async Task<DTOStatusMenus?> GetStatusMenuAsync(int Page)
        {
            return await _IRead.GetAsync(Page);
        }

        public async Task<DTOStatusMenus?> AddStatusMenuAsync(DTOStatusMenusCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }

        public async Task<DTOStatusMenus?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }
        public async Task<bool> DeleteStatusMenuAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

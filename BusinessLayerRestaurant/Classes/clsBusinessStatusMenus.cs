using DataLayerRestaurant;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public class clsStatusMenusDtoContainer : IDTOBStatusMenus
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

    public class clsStatusMenusRepositoryBridge : IInterfaceBStatusMenus
    {
        private IDataStatusMenus _IData;
        public IDataStatusMenus IData
        {
            get => _IData;
            set => _IData = value;
        }

        public clsStatusMenusRepositoryBridge(IDataStatusMenus IData)
        {
            _IData = IData;
        }
    }

    public class clsStatusMenusReader : IReadableBStatusMenus
    {
        private IInterfaceBStatusMenus _Interface;
        public clsStatusMenusReader(IInterfaceBStatusMenus @interface)
        {
            _Interface = @interface;
        }

        public async Task<DTOStatusMenus?> GetAsync(int ID)
        {
            var result = await _Interface.IData.GetStatusMenuAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return result;
        }

        public async Task<List<DTOStatusMenus>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllStatusMenusAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;
        }
    }

    public class clsStatusMenusWriter : IWritableBStatusMenus
    {
        private IInterfaceBStatusMenus _Interface;
        public clsStatusMenusWriter(IInterfaceBStatusMenus @interface)
        {
            _Interface = @interface;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteStatusMenuAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Deleted!");
            }

            return result;
        }

        public async Task<DTOStatusMenus?> UpdateAsync(DTOStatusMenusURequest Request)
        {
            var result = await _Interface.IData.UpdateStatusMenuAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            return result;
        }

        public async Task<DTOStatusMenus?> CreateAsync(DTOStatusMenusCRequest Request)
        {
            var result = await _Interface.IData.AddStatusMenuAsync(Request);  
            if (result == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            return result;
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

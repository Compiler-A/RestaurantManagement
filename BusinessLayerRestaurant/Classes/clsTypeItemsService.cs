#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.TypeItems;


namespace BusinessLayerRestaurant.Classes
{


    public class clsTypeItemsContainer : ITypeItemsServiceContainer
    {
        private IDataTypeItems _IDataTypeItem;
        public IDataTypeItems IData
        {
            get => _IDataTypeItem;
            set => _IDataTypeItem = value;
        }

        public clsTypeItemsContainer(IDataTypeItems Data)
        {
            _IDataTypeItem = Data;
        }
    }

    public class clsTypeItemsReader : ITypeItemsServiceReader
    {
        private ITypeItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsTypeItemsReader(ITypeItemsServiceContainer Interface, IMyLogger logger)
        {
            _Interface = Interface;
            _Logger = logger;
        }
        public async Task<DTOTypeItems?> GetAsync(int ID)
        {
            var result  = await _Interface.IData.GetTypeItemAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"TypeItem Found, Name: {result.Name}", EventLogEntryType.Information);

            return result;
        }
        public async Task<List<DTOTypeItems>> GetAllAsync(int page)
        {
            var result = await _Interface.IData.GetAllTypeItemsAsync(page);
            if (result == null || result.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"TypeItems Found, Count: {result.Count}", EventLogEntryType.Information);

            return result;
        }
    }

    public class clsTypeItemsWriter : ITypeItemsServiceWriter
    {
        private ITypeItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsTypeItemsWriter(ITypeItemsServiceContainer @interface, IMyLogger logger)
        {
            _Interface = @interface;
            _Logger = logger;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interface.IData.DeleteTypeItemAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted");
            }
            _Logger.EventLogs($"TypeItem Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }
        public async Task<DTOTypeItems?> UpdateAsync(DTOTypeItemsURequest Request)
        { 
            var result = await _Interface.IData.UpdateTypeItemAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"TypeItem Updated, Name: {result.Name}", EventLogEntryType.Information);

            return result;
        }

        public async Task<DTOTypeItems?> CreateAsync(DTOTypeItemsCRequest Request)
        {
            var result = await _Interface.IData.AddTypeItemAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"TypeItem Created, Name: {result.Name}", EventLogEntryType.Information);

            return result;
        }
    }



    public class clsTypeItemsService : IBusinessService
    {
        ITypeItemsServiceWriter _IWrite;
        ITypeItemsServiceReader _IRead;

        public clsTypeItemsService( ITypeItemsServiceWriter write,  ITypeItemsServiceReader read)
        {
            _IWrite = write;
            _IRead = read;
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

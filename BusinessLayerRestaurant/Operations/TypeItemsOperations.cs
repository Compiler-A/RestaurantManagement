#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using ContractsLayerRestaurant.DTORequest.TypeItems;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Operations
{


    public class TypeItemsContainer : ITypeItemsServiceContainer
    {
        private ITypeItemsRepository _IDataTypeItem;
        public ITypeItemsRepository IData
        {
            get => _IDataTypeItem;
            set => _IDataTypeItem = value;
        }

        public TypeItemsContainer(ITypeItemsRepository Data)
        {
            _IDataTypeItem = Data;
        }
    }

    public class TypeItemsReader : ITypeItemsServiceReader
    {
        private ITypeItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public TypeItemsReader(ITypeItemsServiceContainer Interface, IMyLogger logger)
        {
            _Interface = Interface;
            _Logger = logger;
        }
        public async Task<TypeItem?> GetAsync(int ID)
        {
            var result  = await _Interface.IData.GetDataAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"TypeItem Found, Name: {result.TypeName}", EventLogEntryType.Information);

            return result;
        }
        public async Task<List<TypeItem>> GetAllAsync(int page)
        {
            var result = await _Interface.IData.GetAllDataAsync(page);
            if (result == null || result.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"TypeItems Found, Count: {result.Count}", EventLogEntryType.Information);

            return result;
        }
    }

    public class TypeItemsWriter : ITypeItemsServiceWriter
    {
        private ITypeItemsServiceContainer _Interface;
        private IMyLogger _Logger;
        public TypeItemsWriter(ITypeItemsServiceContainer @interface, IMyLogger logger)
        {
            _Interface = @interface;
            _Logger = logger;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interface.IData.DeleteDataAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted");
            }
            _Logger.EventLogs($"TypeItem Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }
        public async Task<TypeItem?> UpdateAsync(DTOTypeItemsURequest Request)
        {
            var result = await _Interface.IData.UpdateDataAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"TypeItem Updated, Name: {result.TypeName}", EventLogEntryType.Information);

            return result;
        }

        public async Task<TypeItem?> CreateAsync(DTOTypeItemsCRequest Request)
        {
            var result = await _Interface.IData.CreateDataAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"TypeItem Created, Name: {result.TypeName}", EventLogEntryType.Information);

            return result;
        }
    }
}

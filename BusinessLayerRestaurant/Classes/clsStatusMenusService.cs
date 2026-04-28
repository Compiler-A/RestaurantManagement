#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusMenus;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Classes
{

    public class clsStatusMenusContainer : IStatusMenusServiceContainer
    {
        private IStatusMenusRepository _IData;
        public IStatusMenusRepository IData
        {
            get => _IData;
            set => _IData = value;
        }

        public clsStatusMenusContainer(IStatusMenusRepository IData)
        {
            _IData = IData;
        }
    }

    public class clsStatusMenusReader : IStatusMenusServiceReader
    {
        private IStatusMenusServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsStatusMenusReader(IStatusMenusServiceContainer @interface, IMyLogger logger)
        {
            _Interface = @interface;
            _Logger = logger;
        }

        public async Task<StatusMenu?> GetAsync(int ID)
        {
            var result = await _Interface.IData.GetDataAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"StatusMenu Found, Name: {result.Name}", EventLogEntryType.Information);

            return result;
        }

        public async Task<List<StatusMenu>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllDataAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"StatusMenus Found, Count: {list.Count}", EventLogEntryType.Information);

            return list;
        }
    }

    public class clsStatusMenusWriter : IStatusMenusServiceWriter
    {
        private IStatusMenusServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsStatusMenusWriter(IStatusMenusServiceContainer @interface, IMyLogger logger)
        {
            _Interface = @interface;
            _Logger = logger;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteDataAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"StatusMenu Deleted, ID: {ID}", EventLogEntryType.Information);

            return result;
        }

        public async Task<StatusMenu?> UpdateAsync(DTOStatusMenusURequest Request)
        {
            var result = await _Interface.IData.UpdateDataAsync(Request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"StatusMenu Updated, Name: {result.Name}", EventLogEntryType.Information);

            return result;
        }

        public async Task<StatusMenu?> CreateAsync(DTOStatusMenusCRequest Request)
        {
            var result = await _Interface.IData.CreateDataAsync(Request);  
            if (result == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"StatusMenu Created, Name: {result.Name}", EventLogEntryType.Information);

            return result;
        }
    }

    
    public class clsStatusMenusService : IStatusMenusService
    {
        IStatusMenusServiceReader _IRead;
        IStatusMenusServiceWriter _IWrite;

        public clsStatusMenusService( IStatusMenusServiceWriter Write, IStatusMenusServiceReader Read)
        {
            _IRead = Read;
            _IWrite = Write;
        }


        public async Task<List<StatusMenu>> GetAllAsync(int Page)
        {
            return await _IRead.GetAllAsync(Page);
        }
        public async Task<StatusMenu?> GetAsync(int Page)
        {
            return await _IRead.GetAsync(Page);
        }

        public async Task<StatusMenu?> CreateAsync(DTOStatusMenusCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }

        public async Task<StatusMenu?> UpdateAsync(DTOStatusMenusURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }
        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Classes
{

    public class clsStatusOrdersContainer : IStatusOrdersServiceContainer
    {
        private IStatusOrdersRepository _IDataStatusOrder;
        public clsStatusOrdersContainer(IStatusOrdersRepository IDataStatusOrder)
        {
            this._IDataStatusOrder = IDataStatusOrder;
        }
        public IStatusOrdersRepository IData
        {
            get => this._IDataStatusOrder;
            set => this._IDataStatusOrder = value;
        }
    }

    public class clsStatusOrdersReader : IStatusOrdersServiceReader
    {
        private IStatusOrdersServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsStatusOrdersReader(IStatusOrdersServiceContainer Interface, IMyLogger logger)
        {
            _Interface = Interface;
            _Logger = logger;
        }
        public async Task<List<StatusOrder>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllDataAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"StatusOrders Found, Count: {list.Count}", EventLogEntryType.Information);

            return list;
        }

        public async Task<StatusOrder?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetDataAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"StatusOrder Found, Name: {dto.Name}", EventLogEntryType.Information);

            return dto;
        }
    }
    public class clsStatusOrdersWriter : IStatusOrdersServiceWriter
    {
        private IStatusOrdersServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsStatusOrdersWriter(IStatusOrdersServiceContainer setting, IMyLogger Logger)
        {
            _Interface = setting;
            _Logger = Logger;
        }



        public async Task<StatusOrder?> CreateAsync(DTOStatusOrdersCRequest Request)
        {

            var dto = await _Interface.IData.CreateDataAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"StatusOrder Created, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }
        public async Task<StatusOrder?> UpdateAsync(DTOStatusOrdersURequest Request)
        {  
            var dto = await _Interface.IData.UpdateDataAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"StatusOrder Updated, Name: {dto.Name}", EventLogEntryType.Information);

            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interface.IData.DeleteDataAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"StatusOrder Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }
    }


    public class clsStatusOrdersService : IStatusOrdersService
    {
        private IStatusOrdersServiceReader _IRead;
        private IStatusOrdersServiceWriter _IWrite;

        public clsStatusOrdersService(IStatusOrdersServiceReader read, IStatusOrdersServiceWriter write)
        {
            _IRead = read;
            _IWrite = write;
        }


        public async Task<List<StatusOrder>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<StatusOrder?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<StatusOrder?> CreateAsync(DTOStatusOrdersCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<StatusOrder?> UpdateAsync(DTOStatusOrdersURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }

    }

}

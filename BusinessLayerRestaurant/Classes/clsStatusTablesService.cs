#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusTables;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Classes
{


    public class clsStatusTablesContainer : IStatusTablesServiceContainer
    {
        private IStatusTablesRepository _IStatusTable;
        public IStatusTablesRepository IData
        {
            get => _IStatusTable;
            set => _IStatusTable = value;
        }
        public clsStatusTablesContainer(IStatusTablesRepository iStatusTable)
        {
            _IStatusTable = iStatusTable;
        }
    }

    public class clsStatusTablesReader : IStatusTablesServiceReader
    {
        private IStatusTablesServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsStatusTablesReader(IStatusTablesServiceContainer iInterface, IMyLogger logger)
        {
            _Interface = iInterface;
            _Logger = logger;
        }

        public async Task<StatusTable?> GetAsync(int ID)
        {
            var result = await _Interface.IData.GetDataAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"StatusTable Found, Name: {result.Name}", EventLogEntryType.Information);

            return result;
        }

        public async Task<List<StatusTable>> GetAllAsync(int page)
        {
            var result = await _Interface.IData.GetAllDataAsync(page);
            if (result == null || result.Count == 0)
                throw new KeyNotFoundException("Not Found!");

            _Logger.EventLogs($"StatusTables Found, Count: {result.Count}", EventLogEntryType.Information);
            return result;
        }

        public async Task<bool> isFindAsync(int ID)
        {
            var result = await _Interface.IData.isFindDataAsync(ID);
            if (!result)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"StatusTable Found.", EventLogEntryType.Information);

            return result;
        }

    }

    public class clsStatusTablesWriter : IStatusTablesServiceWriter
    {
        private IStatusTablesServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsStatusTablesWriter(IStatusTablesServiceContainer setting, IMyLogger logger)
        {
            _Interface = setting;
            _Logger = logger;
        }

        public async Task<StatusTable?> CreateAsync(DTOStatusTablesCRequest Request)
        {

            var dto = await _Interface.IData.CreateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"StatusTable Created, Name: {dto.Name}", EventLogEntryType.Information);
                return dto;
            }
            throw new InvalidOperationException("Not Created!");
        }
        public async Task<StatusTable?> UpdateAsync(DTOStatusTablesURequest Request)
        {

            var dto = await _Interface.IData.UpdateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"StatusTable Updated, Name: {dto.Name}", EventLogEntryType.Information);
                return dto;
            }
            throw new InvalidOperationException("Not Updated!");
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteDataAsync(ID);
            if (!result)
            {
                _Logger.EventLogs($"StatusTable Deleted, ID: {ID}", EventLogEntryType.Information);
                throw new InvalidOperationException("Not Deleted!");
            }
            return result;
        }
    }



    public class clsStatusTablesService : IStatusTablesService
    {
        private IStatusTablesServiceReader _IRead;
        private IStatusTablesServiceWriter _IWrite;

        public clsStatusTablesService(IStatusTablesServiceReader read, IStatusTablesServiceWriter write)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<StatusTable>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<StatusTable?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }

        public async Task<bool> isFindAsync(int id)
        {
            return await _IRead.isFindAsync(id);
        }


        public async Task<StatusTable?> CreateAsync(DTOStatusTablesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<StatusTable?> UpdateAsync(DTOStatusTablesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}
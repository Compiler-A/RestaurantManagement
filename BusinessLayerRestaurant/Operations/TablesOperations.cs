#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using ContractsLayerRestaurant.DTORequest.Tables;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Operations
{


    public class TablesContainer : ITablesServiceContainer
    {
        private ITablesRepository _IDataTable;
        public ITablesRepository IData
        {
            get => _IDataTable;
            set => _IDataTable = value;
        }
        private IStatusTablesService _IBusinessStatusTable;
        public IStatusTablesService IBusinessStatusTable
        {
            get => _IBusinessStatusTable;
            set => _IBusinessStatusTable = value;
        }

        public TablesContainer(ITablesRepository @Data, IStatusTablesService @IBusinessStatusTable)
        {
            _IDataTable = @Data;
            _IBusinessStatusTable = @IBusinessStatusTable;
        }
    }




    public class TablesReader :  ITablesServiceReader
    {
        private ITablesServiceContainer _Interface;
        private IMyLogger _Logger;
        public TablesReader(ITablesServiceContainer @interface,IMyLogger Logger)
        {
            _Interface = @interface;
            _Logger = Logger;
        }


        public async Task<List<Table>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllDataAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;
        }

        public async Task<Table?> GetAsync(int id)
        {
            var dto = await _Interface.IData.GetDataAsync(id);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Table Found, Name: {dto.TableNumber}", EventLogEntryType.Information);

            return dto;
        }

        public async Task<List<Table>> GetAllAsync()
        {
            var list = await _Interface.IData.GetAllDataAsync();
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;
        }
        public async Task<List<Table>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request)
        {
            var list = await _Interface.IData.GetFilterStatusDataAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;
        }
        public async Task<List<Table>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request)
        {
            var list = await _Interface.IData.GetFilterSeatDataAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;

        }
        public async Task<Table?> GetByNameAsync(string tableNumber)
        {
            var dto = await _Interface.IData.GetDataByNameAsync(tableNumber);
            if (dto  == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Table Found, Name: ${dto.TableNumber}", EventLogEntryType.Information);

            return dto;
        }
        public async Task<List<Table>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            var list = await _Interface.IData.GetFilterStatusAndSeatDataAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;
        }
        public async Task<List<Table>> GetAllAvailablesAsync()
        {
            var list = await _Interface.IData.GetAllDataAvailablesAsync();
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;
        }
    }

    public class TablesWriter : ITablesServiceWriter
    {
        private ITablesServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public TablesWriter
            (IMyLogger Logger, ITablesServiceContainer @interface)
        {
            _Logger = Logger;
            _Interfaces = @interface;
        }
        public async Task<Table?> CreateAsync(DTOTablesCRequest Request)
        {

            var dto = await _Interfaces.IData.CreateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"Table Created, Name: {dto.TableNumber}", EventLogEntryType.Information);

                return dto;
            }
            throw new InvalidOperationException("Not Created!");
        }

        public async Task<Table?> UpdateAsync(DTOTablesURequest Request)
        {

            var dto = await _Interfaces.IData.UpdateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"Table Updated, Name: {dto.TableNumber}", EventLogEntryType.Information);

                return dto;
            }
            throw new InvalidOperationException("Not Updated!");
        }


        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interfaces.IData.DeleteDataAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"Table Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }

    }
}

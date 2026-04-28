#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Tables;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Classes
{


    public class clsTablesContainer : ITablesServiceContainer
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

        public clsTablesContainer(ITablesRepository @Data, IStatusTablesService @IBusinessStatusTable)
        {
            _IDataTable = @Data;
            _IBusinessStatusTable = @IBusinessStatusTable;
        }
    }

    public class clsStatusTableLoader : ITablesServiceComposition
    {
        private IStatusTablesService _IData;
        public clsStatusTableLoader(IStatusTablesService iData)
        {
            _IData = iData;
        }

        public async Task LoadDataAsync(Table item)
        {
            item.StatusTable = await _IData.GetAsync(item.StatusTableID);
        }
    }

    public class clsCompositionTablesLoader : ITablesServiceComposition
    {
        private IEnumerable<ITablesServiceComposition> _loaders;
        public clsCompositionTablesLoader
            (IEnumerable<ITablesServiceComposition> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(Table item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsTablesReader : clsCompositionTablesLoader, ITablesServiceReader
    {
        private ITablesServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsTablesReader(ITablesServiceContainer @interface,IMyLogger Logger ,IEnumerable<ITablesServiceComposition> loaders)
            : base(loaders)
        {
            _Interface = @interface;
            _Logger = Logger;
        }

        private async Task<List<Table>> _LoadAsync(List<Table> list)
        {
            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"Tables Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }

        public async Task<List<Table>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllDataAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return await _LoadAsync(list);
        }

        public async Task<Table?> GetAsync(int id)
        {
            var dto = await _Interface.IData.GetDataAsync(id);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Table Found, Name: {dto.Name}", EventLogEntryType.Information);

            await LoadDataAsync(dto);
            return dto;
        }

        public async Task<List<Table>> GetAllAsync()
        {
            var list = await _Interface.IData.GetAllDataAsync();
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return await _LoadAsync(list);
        }
        public async Task<List<Table>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request)
        {
            var list = await _Interface.IData.GetFilterStatusDataAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return await _LoadAsync(list);
        }
        public async Task<List<Table>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request)
        {
            var list = await _Interface.IData.GetFilterSeatDataAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return await _LoadAsync(list);

        }
        public async Task<Table?> GetByNameAsync(string tableNumber)
        {
            var dto = await _Interface.IData.GetDataByNameAsync(tableNumber);
            if (dto  == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Table Found, Name: ${dto.Name}", EventLogEntryType.Information);

            await LoadDataAsync(dto);
            return dto;
        }
        public async Task<List<Table>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            var list = await _Interface.IData.GetFilterStatusAndSeatDataAsync(Request);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return await _LoadAsync(list);
        }
        public async Task<List<Table>> GetAllAvailablesAsync()
        {
            var list = await _Interface.IData.GetAllDataAvailablesAsync();
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return await _LoadAsync(list);
        }
    }

    public class clsTablesWriter : clsCompositionTablesLoader , ITablesServiceWriter
    {
        private ITablesServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public clsTablesWriter
            (IMyLogger Logger, ITablesServiceContainer @interface, IEnumerable<ITablesServiceComposition> loader) : base(loader)
        {
            _Logger = Logger;
            _Interfaces = @interface;
        }
        public async Task<Table?> CreateAsync(DTOTablesCRequest Request)
        {

            var dto = await _Interfaces.IData.CreateDataAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                _Logger.EventLogs($"Table Created, Name: {dto.Name}", EventLogEntryType.Information);

                return dto;
            }
            throw new InvalidOperationException("Not Created!");
        }

        public async Task<Table?> UpdateAsync(DTOTablesURequest Request)
        {

            var dto = await _Interfaces.IData.UpdateDataAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                _Logger.EventLogs($"Table Updated, Name: {dto.Name}", EventLogEntryType.Information);

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


    public class clsTablesService : ITablesService
    {
        private ITablesServiceContainer _Interfaces;
        private ITablesServiceWriter _IWrite;
        private ITablesServiceReader _IRead;

        public clsTablesService( ITablesServiceContainer table, ITablesServiceWriter write, 
            ITablesServiceReader read)
        {
            _Interfaces = table;
            _IWrite = write;
            _IRead = read;
        }


        public IStatusTablesService IStatusTable
        {
            get => _Interfaces.IBusinessStatusTable;
            set => _Interfaces.IBusinessStatusTable = value;
        }

        public async Task<List<Table>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }
        public async Task<Table?> GetAsync(int id)
        {
            return await _IRead.GetAsync(id);
        }
        public async Task<List<Table>> GetAllAsync()
        {
            return await _IRead.GetAllAsync();
        }
        public async Task<List<Table>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request)
        {
            return await _IRead.GetFilter1Async(Request);
        }
        public async Task<List<Table>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request)
        {
           return await _IRead.GetFilter2Async(Request);
        }
        public async Task<Table?> GetByNameAsync(string tableNumber)
        {
            return await _IRead.GetByNameAsync(tableNumber);
        }
        public async Task<List<Table>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            return await _IRead.GetFilter3Async(Request);
        }
        public async Task<List<Table>> GetAllAvailablesAsync()
        {
            return await _IRead.GetAllAvailablesAsync();
        }
        public async Task<Table?> CreateAsync(DTOTablesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<Table?> UpdateAsync(DTOTablesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }
        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

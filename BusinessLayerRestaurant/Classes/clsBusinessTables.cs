using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayerRestaurant
{
    public class clsTablesDtoContainer : IDTOBTables
    {
        private DTOTablesCRequest? _CRequest;
        public DTOTablesCRequest? CreateRequest
        {
            get => _CRequest;
            set => _CRequest = value;
        }

        private DTOTablesURequest? _URequest;
        public DTOTablesURequest? UpdateRequest
        {
            get => _URequest;
            set => _URequest = value;
        }
    }

    public class clsTablesRepositoryBridge : IInterfaceBTables
    {
        private IDataTables _IDataTable;
        public IDataTables IData
        {
            get => _IDataTable;
            set => _IDataTable = value;
        }
        private IBusinessStatusTables _IBusinessStatusTable;
        public IBusinessStatusTables IBusinessStatusTable
        {
            get => _IBusinessStatusTable;
            set => _IBusinessStatusTable = value;
        }

        public clsTablesRepositoryBridge(IDataTables @Data, IBusinessStatusTables @IBusinessStatusTable)
        {
            _IDataTable = @Data;
            _IBusinessStatusTable = @IBusinessStatusTable;
        }
    }

    public class clsStatusTableLoader : ICompositionBTables
    {
        private IBusinessStatusTables _IData;
        public clsStatusTableLoader(IBusinessStatusTables iData)
        {
            _IData = iData;
        }

        public async Task LoadDataAsync(DTOTables item)
        {
            item.StatusTable = await _IData.GetStatusTableAsync(item.StatusTableID);
        }
    }

    public class clsCompositionTablesLoader : ICompositionBTables
    {
        private IEnumerable<ICompositionBTables> _loaders;
        public clsCompositionTablesLoader
            (IEnumerable<ICompositionBTables> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(DTOTables item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsTablesReader : clsCompositionTablesLoader, IReadableBTables
    {
        private IInterfaceBTables _Interface;
        public clsTablesReader(IInterfaceBTables @interface, IEnumerable<ICompositionBTables> loaders)
            : base(loaders)
        {
            _Interface = @interface;
        }

        private async Task<List<DTOTables>> _LoadAsync(List<DTOTables> list)
        {
            foreach (var item in list)
            {
                await LoadDataAsync(item);
            }
            return list;
        }

        public async Task<List<DTOTables>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAlltablesAsync(page);
            return await _LoadAsync(list);
        }

        public async Task<DTOTables?> GetAsync(int id)
        {
            var dto = await _Interface.IData.GetTableAsync(id);
            if (dto == null)
            {
                return null;
            }

            await LoadDataAsync(dto);
            return dto;
        }

        public async Task<List<DTOTables>> GetAllAsync()
        {
            var list = await _Interface.IData.GetAlltablesAsync();
            
            return await _LoadAsync(list);
        }
        public async Task<List<DTOTables>> GetFilter1Async(int Page, int StatusTable)
        {
            var list  = await _Interface.IData.GetFilterStatusTablesAsync(Page, StatusTable);
            return await _LoadAsync(list);
        }
        public async Task<List<DTOTables>> GetFilter2Async(int Page, int Seats)
        {
            var list = await _Interface.IData.GetFilterSeatTablesAsync(Page, Seats);
            return await _LoadAsync(list);
        }
        public async Task<DTOTables?> GetByNameAsync(string tableNumber)
        {
            var dto = await _Interface.IData.GetTableByNameAsync(tableNumber);
            if (dto  == null)
            {
                return null;
            }
            await LoadDataAsync(dto);
            return dto;
        }
        public async Task<List<DTOTables>> GetFilter3Async(int page, int StatusTable, int SeatNumber)
        {
            var list = await _Interface.IData.GetFilterStatusAndSeatTablesAsync(page, StatusTable, SeatNumber);
            return await _LoadAsync(list);
        }
        public async Task<List<DTOTables>> GetAllAvailablesAsync()
        {
            var list = await _Interface.IData.GetAllTablesAvailablesAsync();
            return await _LoadAsync(list);
        }
    }

    public class clsTablesWriter : clsCompositionTablesLoader , IWritableBTables
    {
        private IDTOBTables _Dtos;
        private IInterfaceBTables _Interfaces;
        public clsTablesWriter
            (IDTOBTables dto, IInterfaceBTables @interface, IEnumerable<ICompositionBTables> loader) : base(loader)
        {
            _Dtos = dto;
            _Interfaces = @interface;
        }
        public async Task<DTOTables?> CreateAsync(DTOTablesCRequest Request)
        {
            if (Request == null)
            { return null; }
            var dto = await _Interfaces.IData.AddTableAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                return dto;
            }
            return null;
        }

        public async Task<DTOTables?> UpdateAsync(DTOTablesURequest Request)
        {
            if (Request == null)
            {
                return null;
            }
            var dto = await _Interfaces.IData.UpdateTableAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                return dto;
            }
            return null;
        }


        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interfaces.IData.DeleteTableAsync(ID);
        }

    }


    public class clsBusinessTables : IBusinessTables
    {
        private IDTOBTables _Dtos;
        private IInterfaceBTables _Interfaces;
        private IWritableBTables _IWrite;
        private IReadableBTables _IRead;

        public clsBusinessTables(IDTOBTables dto, IInterfaceBTables table, IWritableBTables write, 
            IReadableBTables read)
        {
            _Dtos = dto;
            _Interfaces = table;
            _IWrite = write;
            _IRead = read;
        }

        public DTOTablesCRequest? CreateRequest
        {
            get => _Dtos.CreateRequest;
            set => _Dtos.CreateRequest = value;
        }

        public DTOTablesURequest? UpdateRequest
        {
            get => _Dtos.UpdateRequest;
            set => _Dtos.UpdateRequest = value;
        }

        public IBusinessStatusTables IStatusTable
        {
            get => _Interfaces.IBusinessStatusTable;
            set => _Interfaces.IBusinessStatusTable = value;
        }

        public async Task<List<DTOTables>> GetAllTablesAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }
        public async Task<DTOTables?> GetTableAsync(int id)
        {
            return await _IRead.GetAsync(id);
        }
        public async Task<List<DTOTables>> GetAllTablesAsync()
        {
            return await _IRead.GetAllAsync();
        }
        public async Task<List<DTOTables>> GetTablesFilter1Async(int Page, int StatusTable)
        {
            return await _IRead.GetFilter1Async(Page, StatusTable);
        }
        public async Task<List<DTOTables>> GetTablesFilter2Async(int Page, int Seats)
        {
           return await _IRead.GetFilter2Async(Page, Seats);
        }
        public async Task<DTOTables?> GetTableByNameAsync(string tableNumber)
        {
            return await _IRead.GetByNameAsync(tableNumber);
        }
        public async Task<List<DTOTables>> GetTablesFilter3Async(int page, int StatusTable, int SeatNumber)
        {
            return await _IRead.GetFilter3Async(page, StatusTable, SeatNumber);
        }
        public async Task<List<DTOTables>> GetAllTablesAvailablesAsync()
        {
            return await _IRead.GetAllAvailablesAsync();
        }
        public async Task<DTOTables?> AddTableAsync(DTOTablesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }
        public async Task<bool> DeleteTableAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

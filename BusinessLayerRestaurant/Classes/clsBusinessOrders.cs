#pragma warning disable CA1416 // Validate platform compatibility
using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;


namespace BusinessLayerRestaurant
{

    public class clsOrdersRepositoryBridge : IInterfaceBOrders
    {
        private IDataOrders _Iorder;
        private IBusinessStatusOrders _IStatusOrder;
        private IBusinessEmployees _IBusinessEmployee;
        private IBusinessTables _IBusinessTable;
        public IDataOrders IData
        {
            get => _Iorder;
            set => _Iorder = value;
        }
        public IBusinessStatusOrders IBusinessStatusOrder
        {
            get => _IStatusOrder;
            set => _IStatusOrder = value;
        }
        public IBusinessEmployees IBusinessEmployee
        {
            get => _IBusinessEmployee;
            set => _IBusinessEmployee = value;
        }
        public IBusinessTables IBusinessTable
        {
            get => _IBusinessTable;
            set => _IBusinessTable = value;
        }
        public clsOrdersRepositoryBridge
            (IDataOrders Order, IBusinessEmployees Business, IBusinessStatusOrders StatusOrder, IBusinessTables Table)
        {
            _IStatusOrder = StatusOrder;
            _IBusinessEmployee = Business;
            _IBusinessTable = Table;
            _Iorder = Order;
        }
    }
    public class clsOrdersDtoContainer : IDTOBOrders
    {
        private DTOOrderCRequest? _dtoOrderRequest { get; set; }
        public DTOOrderCRequest? CreateRequest
        {
            get => _dtoOrderRequest;
            set => _dtoOrderRequest = value;
        }
        private DTOOrderURequest? _dtoOrderUpdateRequest { get; set; }
        public DTOOrderURequest? UpdateRequest
        {
            get => _dtoOrderUpdateRequest;
            set => _dtoOrderUpdateRequest = value;
        }
    }
    public class  clsStatusOrderLoader : ICompositionBOrders
    {
        private IBusinessStatusOrders _Status;
        public clsStatusOrderLoader(IBusinessStatusOrders Status)
        {
            _Status = Status;
        }

        public async Task LoadDataAsync(DTOOrders item)
        {
            item.statusOrders = await _Status.GetStatusOrdersAsync(item.StatusOrderID);
        }
    }
    public class clsEmployeeLoader : ICompositionBOrders 
    {
        private IBusinessEmployees _Employee;
        public clsEmployeeLoader(IBusinessEmployees Employee)
        {
            _Employee = Employee;
        }
        public async Task LoadDataAsync(DTOOrders item) {
            item.employees = await _Employee.GetEmployeeAsync(item.EmployerID);
        }
    }
    public class clsTableLoader : ICompositionBOrders
    {
        private IBusinessTables _Table;
        public clsTableLoader(IBusinessTables Table)
        {
            _Table = Table;
        }
        public async Task LoadDataAsync(DTOOrders item)
        {
            item.tables = await _Table.GetTableAsync(item.TableID);
        }
    }
    public class clsCompositionOrdersLoader
    {
        private IEnumerable<ICompositionBOrders> _loaders;
        public clsCompositionOrdersLoader
            (IEnumerable<ICompositionBOrders> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(DTOOrders item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }

    }
    public class clsOrdersReader : clsCompositionOrdersLoader, IReadableBOrders
    {
        private IInterfaceBOrders _Interfaces;
        private IMyLogger _Logger;
        public clsOrdersReader
            (IInterfaceBOrders Interfaces, IEnumerable<ICompositionBOrders> loaders, IMyLogger logger) : base(loaders)
        {
            _Interfaces = Interfaces;
            _Logger = logger;
        }

        public async Task<List<DTOOrders>> GetAllAsync(int page)
        {
            var ldto = await _Interfaces.IData.GetAllOrdersAsync(page);
            if (ldto == null || ldto.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            foreach (var item in ldto)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"Orders Found, Count: {ldto.Count}", EventLogEntryType.Information);


            return ldto;
        }

        public async Task<DTOOrders?> GetAsync(int ID)
        {
            var dto = await _Interfaces.IData.GetOrderAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            await LoadDataAsync(dto);
            _Logger.EventLogs($"Order Found, ID: {dto.ID}", EventLogEntryType.Information);

            return dto;
        }

        public async Task<List<DTOOrders>?> GetFilterAsync
            (DTOOrderFilterRequest Request)
        {
            var koko = await _Interfaces.IData.GetFilterOrderAsync(Request);
            if (koko == null || koko.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            foreach (var item in koko)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"Orders Found, Count: {koko.Count}", EventLogEntryType.Information);

            return koko;
        }
    }
    public class clsOrdersWriter : clsCompositionOrdersLoader, IWritableBOrders
    {
        private IInterfaceBOrders _Interfaces;
        private IMyLogger _Logger;
        public clsOrdersWriter
            (IMyLogger Logger, IInterfaceBOrders Interfaces, IEnumerable<ICompositionBOrders> loaders) : base(loaders)
        {
            _Logger = Logger;
            _Interfaces = Interfaces;
        }

        public async Task<DTOOrders?> CreateAsync(DTOOrderCRequest Request)
        {

            var dto = await _Interfaces.IData.AddOrderAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                _Logger.EventLogs($"Order Created, ID: {dto.ID}", EventLogEntryType.Information);
                return dto;
            }
            throw new InvalidOperationException("Not Created!");
        }

        public async Task<DTOOrders?> UpdateAsync(DTOOrderURequest Request)
        {
            var dto = await _Interfaces.IData.UpdateOrderAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                _Logger.EventLogs($"Order Updated, ID: {dto.ID}", EventLogEntryType.Information);
                return dto;
            }
            throw new InvalidOperationException("Not Updated!");
        }


        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interfaces.IData.DeleteOrderAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"Order Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }
    }


    public class clsBusinessOrders : IBusinessOrders
    {
        private IDTOBOrders _IProperties;
        private IInterfaceBOrders _Interface;
        private IWritableBOrders _IWrite;
        private IReadableBOrders _Read;

        public clsBusinessOrders(
            IDTOBOrders Properties,
            IWritableBOrders Write,             
            IReadableBOrders read,
            IInterfaceBOrders Interface)
        {
            _IProperties = Properties;
            _IWrite = Write;
            _Read = read;
            _Interface = Interface;
        }

        public DTOOrderCRequest? CreateRequest
        {
            get => _IProperties.CreateRequest;
            set => _IProperties.CreateRequest = value;
        }

        public DTOOrderURequest? UpdateRequest
        {
            get => _IProperties.UpdateRequest;
            set => _IProperties.UpdateRequest = value;
        }
        public IBusinessStatusOrders IStatusOrder
        {
            get => _Interface.IBusinessStatusOrder;
            set => _Interface.IBusinessStatusOrder = value;
        }
        public IBusinessEmployees IEmployee
        {
            get => _Interface.IBusinessEmployee;
            set => _Interface.IBusinessEmployee = value;
        }
        public IBusinessTables ITable
        {
            get => _Interface.IBusinessTable;
            set => _Interface.IBusinessTable = value;
        }

        public async Task<List<DTOOrders>> GetAllOrdersAsync(int page)
        {
            return await _Read.GetAllAsync(page);
        }

        public async Task<DTOOrders?> GetOrderAsync(int ID)
        {
            return await _Read.GetAsync(ID);
        }

        public async Task<List<DTOOrders>?> GetFilterOrdersAsync(DTOOrderFilterRequest Request)
        {
            return await _Read.GetFilterAsync(Request);
        }
        public async Task<DTOOrders?> AddOrderAsync(DTOOrderCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOOrders?> UpdateOrderAsync(DTOOrderURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteOrderAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}


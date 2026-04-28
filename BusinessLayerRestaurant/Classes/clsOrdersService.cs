#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Orders;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Classes
{

    public class clsOrdersContainer : IOrdersServiceContainer
    {
        private IOrdersRepository _Iorder;
        private IStatusOrdersService _IStatusOrder;
        private IEmployeesService _IBusinessEmployee;
        private ITablesService _IBusinessTable;
        public IOrdersRepository IData
        {
            get => _Iorder;
            set => _Iorder = value;
        }
        public IStatusOrdersService IBusinessStatusOrder
        {
            get => _IStatusOrder;
            set => _IStatusOrder = value;
        }
        public IEmployeesService IBusinessEmployee
        {
            get => _IBusinessEmployee;
            set => _IBusinessEmployee = value;
        }
        public ITablesService IBusinessTable
        {
            get => _IBusinessTable;
            set => _IBusinessTable = value;
        }
        public clsOrdersContainer
            (IOrdersRepository Order, IEmployeesService Business, IStatusOrdersService StatusOrder, ITablesService Table)
        {
            _IStatusOrder = StatusOrder;
            _IBusinessEmployee = Business;
            _IBusinessTable = Table;
            _Iorder = Order;
        }
    }

    public class  clsStatusOrderLoader : IOrdersServiceComposition
    {
        private IStatusOrdersService _Status;
        public clsStatusOrderLoader(IStatusOrdersService Status)
        {
            _Status = Status;
        }

        public async Task LoadDataAsync(Order item)
        {
            item.statusOrders = await _Status.GetStatusOrdersAsync(item.StatusOrderID);
        }
    }
    public class clsEmployeeLoader : IOrdersServiceComposition 
    {
        private IEmployeesService _Employee;
        public clsEmployeeLoader(IEmployeesService Employee)
        {
            _Employee = Employee;
        }
        public async Task LoadDataAsync(Order item) {
            item.employees = await _Employee.GetEmployeeAsync(item.EmployeeID);
        }
    }
    public class clsTableLoader : IOrdersServiceComposition
    {
        private ITablesService _Table;
        public clsTableLoader(ITablesService Table)
        {
            _Table = Table;
        }
        public async Task LoadDataAsync(Order item)
        {
            item.tables = await _Table.GetTableAsync(item.TableID);
        }
    }
    public class clsCompositionOrdersLoader
    {
        private IEnumerable<IOrdersServiceComposition> _loaders;
        public clsCompositionOrdersLoader
            (IEnumerable<IOrdersServiceComposition> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(Order item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }

    }
    public class clsOrdersReader : clsCompositionOrdersLoader, IOrdersServiceReader
    {
        private IOrdersServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public clsOrdersReader
            (IOrdersServiceContainer Interfaces, IEnumerable<IOrdersServiceComposition> loaders, IMyLogger logger) : base(loaders)
        {
            _Interfaces = Interfaces;
            _Logger = logger;
        }

        public async Task<List<Order>> GetAllAsync(int page)
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

        public async Task<Order?> GetAsync(int ID)
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

        public async Task<List<Order>?> GetFilterAsync
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
    public class clsOrdersWriter : clsCompositionOrdersLoader, IOrdersServiceWriter
    {
        private IOrdersServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public clsOrdersWriter
            (IMyLogger Logger, IOrdersServiceContainer Interfaces, IEnumerable<IOrdersServiceComposition> loaders) : base(loaders)
        {
            _Logger = Logger;
            _Interfaces = Interfaces;
        }

        public async Task<Order?> CreateAsync(DTOOrderCRequest Request)
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

        public async Task<Order?> UpdateAsync(DTOOrderURequest Request)
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


    public class clsOrdersService : IOrdersService
    {
        private IOrdersServiceContainer _Interface;
        private IOrdersServiceWriter _IWrite;
        private IOrdersServiceReader _Read;

        public clsOrdersService(
            IOrdersServiceWriter Write,             
            IOrdersServiceReader read,
            IOrdersServiceContainer Interface)
        {
            _IWrite = Write;
            _Read = read;
            _Interface = Interface;
        }


        public IStatusOrdersService IStatusOrder
        {
            get => _Interface.IBusinessStatusOrder;
            set => _Interface.IBusinessStatusOrder = value;
        }
        public IEmployeesService IEmployee
        {
            get => _Interface.IBusinessEmployee;
            set => _Interface.IBusinessEmployee = value;
        }
        public ITablesService ITable
        {
            get => _Interface.IBusinessTable;
            set => _Interface.IBusinessTable = value;
        }

        public async Task<List<Order>> GetAllOrdersAsync(int page)
        {
            return await _Read.GetAllAsync(page);
        }

        public async Task<Order?> GetOrderAsync(int ID)
        {
            return await _Read.GetAsync(ID);
        }

        public async Task<List<Order>?> GetFilterOrdersAsync(DTOOrderFilterRequest Request)
        {
            return await _Read.GetFilterAsync(Request);
        }
        public async Task<Order?> AddOrderAsync(DTOOrderCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<Order?> UpdateOrderAsync(DTOOrderURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteOrderAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}


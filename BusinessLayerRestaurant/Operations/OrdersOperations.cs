#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using ContractsLayerRestaurant.DTORequest.Orders;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Operations
{

    public class OrdersContainer : IOrdersServiceContainer
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
        public OrdersContainer
            (IOrdersRepository Order, IEmployeesService Business, IStatusOrdersService StatusOrder, ITablesService Table)
        {
            _IStatusOrder = StatusOrder;
            _IBusinessEmployee = Business;
            _IBusinessTable = Table;
            _Iorder = Order;
        }
    }


    public class OrdersReader : IOrdersServiceReader
    {
        private IOrdersServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public OrdersReader
            (IOrdersServiceContainer Interfaces, IMyLogger logger) 
        {
            _Interfaces = Interfaces;
            _Logger = logger;
        }

        public async Task<List<Order>> GetAllAsync(int page)
        {
            var ldto = await _Interfaces.IData.GetAllDataAsync(page);
            if (ldto == null || ldto.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Orders Found, Count: {ldto.Count}", EventLogEntryType.Information);
            return ldto;
        }

        public async Task<Order?> GetAsync(int ID)
        {
            var dto = await _Interfaces.IData.GetDataAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Order Found, ID: {dto.OrderID}", EventLogEntryType.Information);

            return dto;
        }

        public async Task<List<Order>?> GetFilterAsync
            (DTOOrderFilterRequest Request)
        {
            var koko = await _Interfaces.IData.GetFilterDataAsync(Request);
            if (koko == null || koko.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Orders Found, Count: {koko.Count}", EventLogEntryType.Information);

            return koko;
        }
    }
    public class OrdersWriter :  IOrdersServiceWriter
    {
        private IOrdersServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public OrdersWriter
            (IMyLogger Logger, IOrdersServiceContainer Interfaces)
        {
            _Logger = Logger;
            _Interfaces = Interfaces;
        }

        public async Task<Order?> CreateAsync(DTOOrderCRequest Request)
        {

            var dto = await _Interfaces.IData.CreateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"Order Created, ID: {dto.OrderID}", EventLogEntryType.Information);
                return dto;
            }
            throw new InvalidOperationException("Not Created!");
        }

        public async Task<Order?> UpdateAsync(DTOOrderURequest Request)
        {
            var dto = await _Interfaces.IData.UpdateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"Order Updated, ID: {dto.OrderID}", EventLogEntryType.Information);
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
            _Logger.EventLogs($"Order Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }
    }
}


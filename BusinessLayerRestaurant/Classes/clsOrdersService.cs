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


    public class clsOrdersReader : IOrdersServiceReader
    {
        private IOrdersServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public clsOrdersReader
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
            _Logger.EventLogs($"Order Found, ID: {dto.ID}", EventLogEntryType.Information);

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
    public class clsOrdersWriter :  IOrdersServiceWriter
    {
        private IOrdersServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public clsOrdersWriter
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
                _Logger.EventLogs($"Order Created, ID: {dto.ID}", EventLogEntryType.Information);
                return dto;
            }
            throw new InvalidOperationException("Not Created!");
        }

        public async Task<Order?> UpdateAsync(DTOOrderURequest Request)
        {
            var dto = await _Interfaces.IData.UpdateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"Order Updated, ID: {dto.ID}", EventLogEntryType.Information);
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

        public async Task<List<Order>> GetAllAsync(int page)
        {
            return await _Read.GetAllAsync(page);
        }

        public async Task<Order?> GetAsync(int ID)
        {
            return await _Read.GetAsync(ID);
        }

        public async Task<List<Order>?> GetFilterAsync(DTOOrderFilterRequest Request)
        {
            return await _Read.GetFilterAsync(Request);
        }
        public async Task<Order?> CreateAsync(DTOOrderCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<Order?> UpdateAsync(DTOOrderURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}


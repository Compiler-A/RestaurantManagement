#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DomainLayer.Entities;



namespace BusinessLayerRestaurant.Operations
{

    public class OrderDetailsContainer : IOrderDetailsServiceContainer
    {
        private IOrderDetailsRepository _IDataOrderDetail;
        public IOrderDetailsRepository IData
        {
            get => _IDataOrderDetail;
            set => _IDataOrderDetail = value;
        }

        private IOrdersService _IBusinessOrder;
        public IOrdersService IBusinessOrder
        {
            get => _IBusinessOrder;
            set => _IBusinessOrder = value;
        }
        private IMenuItemsService _IBusinessMenuItem;
        public IMenuItemsService IBusinessMenuItem
        {
            get => _IBusinessMenuItem;
            set => _IBusinessMenuItem = value;
        }

        public OrderDetailsContainer(IOrderDetailsRepository iData, IOrdersService iBusinessOrder, IMenuItemsService iBusinessMenuItem)
        {
            _IDataOrderDetail = iData;
            _IBusinessOrder = iBusinessOrder;
            _IBusinessMenuItem = iBusinessMenuItem;
        }
    }


    public class OrderDetailsReader : IOrderDetailsServiceReader
    {
        private IOrderDetailsServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public OrderDetailsReader
            (IOrderDetailsServiceContainer Interfaces,  IMyLogger logger) 
        {
            _Interfaces = Interfaces;
            _Logger = logger;
        }
        public async Task<List<OrderDetail>> GetAllAsync(int page)
        {
            var ldto = await _Interfaces.IData.GetAllDataAsync(page);
            if (ldto == null || ldto.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"OrderDetails Found, Count: {ldto.Count}", EventLogEntryType.Information);
            return ldto;
        }
        public async Task<List<OrderDetail>> GetAllByOrderIDAsync(int orderID)
        {
            var ldto = await _Interfaces.IData.GetAllDataByOrderIDAsync(orderID);
            if (ldto == null || ldto.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }

            _Logger.EventLogs($"OrderDetails By OrderID Found, Count: {ldto.Count}", EventLogEntryType.Information);

            return ldto;
        }

        public async Task<OrderDetail?> GetAsync(int ID)
        {
            var dto = await _Interfaces.IData.GetDataAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"OrderDetail Found, ID: {dto.OrderDetailID}", EventLogEntryType.Information);

            return dto;
        }
    }

    public class OrderDetailsWriter : IOrderDetailsServiceWriter
    {
        private IOrderDetailsServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public OrderDetailsWriter
            (IMyLogger Logger ,IOrderDetailsServiceContainer @interface) 
        {
            _Interfaces = @interface;
            _Logger = Logger;
        }
        public async Task<OrderDetail?> CreateAsync(DTOOrderDetailsCRequest Request)
        {

            var dto = await _Interfaces.IData.CreateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"OrderDetail Created, ID: {dto.OrderDetailID}", EventLogEntryType.Information);
                return dto;
            }

            throw new InvalidOperationException("Not Created!");
        }

        public async Task<OrderDetail?> UpdateAsync(DTOOrderDetailsURequest Request)
        {

            var dto = await _Interfaces.IData.UpdateDataAsync(Request);
            if (dto != null)
            {
                _Logger.EventLogs($"OrderDetail Updated, ID: {dto.OrderDetailID}", EventLogEntryType.Information);
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
            _Logger.EventLogs($"OrderDetail Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }

    }
}

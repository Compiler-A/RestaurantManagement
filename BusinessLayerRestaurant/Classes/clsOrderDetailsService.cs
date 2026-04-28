#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.OrderDetails;



namespace BusinessLayerRestaurant.Classes
{

    public class clsOrderDetailsContainer : IOrderDetailsServiceContainer
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

        public clsOrderDetailsContainer(IOrderDetailsRepository iData, IOrdersService iBusinessOrder, IMenuItemsService iBusinessMenuItem)
        {
            _IDataOrderDetail = iData;
            _IBusinessOrder = iBusinessOrder;
            _IBusinessMenuItem = iBusinessMenuItem;
        }
    }


    public class clsOrderLoader : IOrderDetailsServiceComposition
    {
        IOrdersService _Order;
        public clsOrderLoader(IOrdersService order)
        { 
            _Order = order;
        }
        public async Task LoadDataAsync(OrderDetail item)
        {
            item.Order = await _Order.GetOrderAsync(item.OrderID);
        }
    }
    public class clsMenuItemLoader : IOrderDetailsServiceComposition
    {
        IMenuItemsService _MenuItem;
        public clsMenuItemLoader(IMenuItemsService menuItem)
        {
            _MenuItem = menuItem;
        }
        public async Task LoadDataAsync(OrderDetail item)
        {
            item.Item = await _MenuItem.GetMenuItemAsync(item.ItemID);
        }
    }
    public class clsCompositionOrderDetailsLoader : IOrderDetailsServiceComposition
    {
        private IEnumerable<IOrderDetailsServiceComposition> _loaders;
        public clsCompositionOrderDetailsLoader
            (IEnumerable<IOrderDetailsServiceComposition> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(OrderDetail item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }


    public class clsOrderDetailsReader : clsCompositionOrderDetailsLoader, IOrderDetailsServiceReader
    {
        private IOrderDetailsServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public clsOrderDetailsReader
            (IOrderDetailsServiceContainer Interfaces, IEnumerable<IOrderDetailsServiceComposition> loaders, IMyLogger logger) : base(loaders)
        {
            _Interfaces = Interfaces;
            _Logger = logger;
        }
        public async Task<List<OrderDetail>> GetAllAsync(int page)
        {
            var ldto = await _Interfaces.IData.GetAllOrderDetailsAsync(page);
            if (ldto == null || ldto.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            foreach (var item in ldto)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"OrderDetails Found, Count: {ldto.Count}", EventLogEntryType.Information);
            return ldto;
        }
        public async Task<List<OrderDetail>> GetAllByOrderIDAsync(int orderID)
        {
            var ldto = await _Interfaces.IData.GetAllOrderDetailsByOrderIDAsync(orderID);
            if (ldto == null || ldto.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            foreach (var item in ldto)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"OrderDetails By OrderID Found, Count: {ldto.Count}", EventLogEntryType.Information);

            return ldto;
        }

        public async Task<OrderDetail?> GetAsync(int ID)
        {
            var dto = await _Interfaces.IData.GetOrderDetailAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"OrderDetail Found, ID: {dto.ID}", EventLogEntryType.Information);

            await LoadDataAsync(dto);
            return dto;
        }
    }

    public class clsOrderDetailsWriter : clsCompositionOrderDetailsLoader, IOrderDetailsServiceWriter
    {
        private IOrderDetailsServiceContainer _Interfaces;
        private IMyLogger _Logger;
        public clsOrderDetailsWriter
            (IMyLogger Logger ,IOrderDetailsServiceContainer @interface, IEnumerable<IOrderDetailsServiceComposition> loader) : base(loader)
        {
            _Interfaces = @interface;
            _Logger = Logger;
        }
        public async Task<OrderDetail?> CreateAsync(DTOOrderDetailsCRequest Request)
        {

            var dto = await _Interfaces.IData.AddOrderDetailAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                _Logger.EventLogs($"OrderDetail Created, ID: {dto.ID}", EventLogEntryType.Information);
                return dto;
            }

            throw new InvalidOperationException("Not Created!");
        }

        public async Task<OrderDetail?> UpdateAsync(DTOOrderDetailsURequest Request)
        {

            var dto = await _Interfaces.IData.UpdateOrderDetailAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                _Logger.EventLogs($"OrderDetail Updated, ID: {dto.ID}", EventLogEntryType.Information);
                return dto;
            }

            throw new InvalidOperationException("Not Updated!");
        }


        public async Task<bool> DeleteAsync(int ID)
        {
            var isDeleted = await _Interfaces.IData.DeleteOrderDetailAsync(ID);
            if (!isDeleted)
            {
                throw new InvalidOperationException("Not Deleted!");

            }
            _Logger.EventLogs($"OrderDetail Deleted, ID: {ID}", EventLogEntryType.Information);

            return isDeleted;
        }

    }




    public class clsOrderDetailsService : IOrderDetailsService
    {
        private IOrderDetailsServiceContainer _Interface;
        private IOrderDetailsServiceWriter _IWrite;
        private IOrderDetailsServiceReader _Read;

        public clsOrderDetailsService(
            IOrderDetailsServiceWriter Write,
            IOrderDetailsServiceReader read,
            IOrderDetailsServiceContainer interfaces)
        {
            _IWrite = Write;
            _Read = read;
            _Interface = interfaces;
        }

        public IMenuItemsService IMenuItem
        {
            get => _Interface.IBusinessMenuItem;
            set => _Interface.IBusinessMenuItem = value;
        }
        public IOrdersService IOrder
        {
            get => _Interface.IBusinessOrder;
            set => _Interface.IBusinessOrder = value;
        }

        public Task<List<OrderDetail>> GetAllOrderDetailsAsync(int page)
        {
            return _Read.GetAllAsync(page);
        }

        public async Task<OrderDetail?> GetOrderDetailAsync(int ID)
        {
            return await _Read.GetAsync(ID);
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsByOrderIDAsync(int OrderID) 
        {
            return await _Read.GetAllByOrderIDAsync(OrderID);
        }
        public async Task<OrderDetail?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<OrderDetail?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteOrderDetailAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

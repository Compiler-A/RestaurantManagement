#pragma warning disable CA1416 // Validate platform compatibility
using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace BusinessLayerRestaurant
{

    public class clsOrderDetailsDtoContainer : IDTOBOrderDetails
    {
        private DTOOrderDetailsCRequest? _CreateRequest;
        public DTOOrderDetailsCRequest? CreateRequest
        {
            get => _CreateRequest;
            set => _CreateRequest = value;
        }

        private DTOOrderDetailsURequest? _UpdateRequest;
        public DTOOrderDetailsURequest? UpdateRequest
        {
            get => _UpdateRequest;
            set => _UpdateRequest = value;
        }
    }
    public class clsOrderDetailsRepositoryBridge : IInterfaceBOrderDetails
    {
        private IDataOrderDetails _IDataOrderDetail;
        public IDataOrderDetails IData
        {
            get => _IDataOrderDetail;
            set => _IDataOrderDetail = value;
        }

        private IBusinessOrders _IBusinessOrder;
        public IBusinessOrders IBusinessOrder
        {
            get => _IBusinessOrder;
            set => _IBusinessOrder = value;
        }
        private IBusinessMenuItems _IBusinessMenuItem;
        public IBusinessMenuItems IBusinessMenuItem
        {
            get => _IBusinessMenuItem;
            set => _IBusinessMenuItem = value;
        }

        public clsOrderDetailsRepositoryBridge(IDataOrderDetails iData, IBusinessOrders iBusinessOrder, IBusinessMenuItems iBusinessMenuItem)
        {
            _IDataOrderDetail = iData;
            _IBusinessOrder = iBusinessOrder;
            _IBusinessMenuItem = iBusinessMenuItem;
        }
    }



    public class clsOrderLoader : ICompositionBOrderDetails
    {
        IBusinessOrders _Order;
        public clsOrderLoader(IBusinessOrders order)
        { 
            _Order = order;
        }
        public async Task LoadDataAsync(DTOOrderDetails item)
        {
            item.Order = await _Order.GetOrderAsync(item.OrderID);
        }
    }
    public class clsMenuItemLoader : ICompositionBOrderDetails
    {
        IBusinessMenuItems _MenuItem;
        public clsMenuItemLoader(IBusinessMenuItems menuItem)
        {
            _MenuItem = menuItem;
        }
        public async Task LoadDataAsync(DTOOrderDetails item)
        {
            item.Item = await _MenuItem.GetMenuItemAsync(item.ItemID);
        }
    }
    public class clsCompositionOrderDetailsLoader : ICompositionBOrderDetails
    {
        private IEnumerable<ICompositionBOrderDetails> _loaders;
        public clsCompositionOrderDetailsLoader
            (IEnumerable<ICompositionBOrderDetails> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(DTOOrderDetails item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }


    public class clsOrderDetailsReader : clsCompositionOrderDetailsLoader, IReadableBOrderDetails
    {
        private IInterfaceBOrderDetails _Interfaces;
        private IMyLogger _Logger;
        public clsOrderDetailsReader
            (IInterfaceBOrderDetails Interfaces, IEnumerable<ICompositionBOrderDetails> loaders, IMyLogger logger) : base(loaders)
        {
            _Interfaces = Interfaces;
            _Logger = logger;
        }
        public async Task<List<DTOOrderDetails>> GetAllAsync(int page)
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
        public async Task<List<DTOOrderDetails>> GetAllByOrderIDAsync(int orderID)
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

        public async Task<DTOOrderDetails?> GetAsync(int ID)
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

    public class clsOrderDetailsWriter : clsCompositionOrderDetailsLoader, IWritableBOrderDetails
    {
        private IInterfaceBOrderDetails _Interfaces;
        private IMyLogger _Logger;
        public clsOrderDetailsWriter
            (IMyLogger Logger ,IInterfaceBOrderDetails @interface, IEnumerable<ICompositionBOrderDetails> loader) : base(loader)
        {
            _Interfaces = @interface;
            _Logger = Logger;
        }
        public async Task<DTOOrderDetails?> CreateAsync(DTOOrderDetailsCRequest Request)
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

        public async Task<DTOOrderDetails?> UpdateAsync(DTOOrderDetailsURequest Request)
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




    public class clsBusinessOrderDetails : IBusinessOrderDetails
    {
        private IDTOBOrderDetails _IProperties;
        private IInterfaceBOrderDetails _Interface;
        private IWritableBOrderDetails _IWrite;
        private IReadableBOrderDetails _Read;

        public clsBusinessOrderDetails(
            IDTOBOrderDetails Properties,
            IWritableBOrderDetails Write,
            IReadableBOrderDetails read,
            IInterfaceBOrderDetails interfaces)
        {
            _IProperties = Properties;
            _IWrite = Write;
            _Read = read;
            _Interface = interfaces;
        }

        public DTOOrderDetailsCRequest? CreateRequest
        {
            get => _IProperties.CreateRequest;
            set => _IProperties.CreateRequest = value;
        }

        public DTOOrderDetailsURequest? UpdateRequest
        {
            get => _IProperties.UpdateRequest;
            set => _IProperties.UpdateRequest = value;
        }

        public IBusinessMenuItems IMenuItem
        {
            get => _Interface.IBusinessMenuItem;
            set => _Interface.IBusinessMenuItem = value;
        }
        public IBusinessOrders IOrder
        {
            get => _Interface.IBusinessOrder;
            set => _Interface.IBusinessOrder = value;
        }

        public Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page)
        {
            return _Read.GetAllAsync(page);
        }

        public async Task<DTOOrderDetails?> GetOrderDetailAsync(int ID)
        {
            return await _Read.GetAsync(ID);
        }

        public async Task<List<DTOOrderDetails>> GetAllOrderDetailsByOrderIDAsync(int OrderID) 
        {
            return await _Read.GetAllByOrderIDAsync(OrderID);
        }
        public async Task<DTOOrderDetails?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOOrderDetails?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteOrderDetailAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

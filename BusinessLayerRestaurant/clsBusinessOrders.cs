using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public interface IRedableOrdersBusiness
    {
        Task<List<DTOOrders>> GetAllOrdersAsync(int page);
        Task<DTOOrders?> GetOrderAsync(int ID);
        Task<List<DTOOrders>?> GetFilterOrdersAsync(int Page, int TableID, int EmployeeID, int StatusOrderID);
    }

    public interface IWritableOrdersBusiness
    {
        Task<bool> SaveAsync();
        Task<bool> DeleteAsync(int ID);

    }

    public interface IBusinessOrders : IWritableOrdersBusiness, IRedableOrdersBusiness
    {
        DTOOrders? DTOOrders { get; set; }
        DTOOrderCreateRequest? DTOOrderRequest { get; set; }
        DTOOrderUpdateRequest? DTOOrderUpdateRequest { get; set; }
        IBusinessStatusOrder IBusinessStatusOrder { get; set; }
        IBusinessEmployees IBusinessEmployee { get; set; }

        IDataTablesBusiness IBusinessTable { get; set; }
    }

    public class clsBusinessOrders : IBusinessOrders
    {
        public enum enMode
        {
            Add, Update
        }
        public enMode Mode { get; set; } = enMode.Add;

        private DTOOrders? _dtoOrders { get; set; }
        public DTOOrders? DTOOrders
        {
            get => _dtoOrders;
            set => _dtoOrders = value;
        }
        private DTOOrderCreateRequest? _dtoOrderRequest { get; set; }
        public DTOOrderCreateRequest? DTOOrderRequest
        {
            get => _dtoOrderRequest;
            set => _dtoOrderRequest = value;
        }

        private DTOOrderUpdateRequest? _dtoOrderUpdateRequest { get; set; }
        public DTOOrderUpdateRequest? DTOOrderUpdateRequest
        {
            get => _dtoOrderUpdateRequest;
            set => _dtoOrderUpdateRequest = value;
        }

        IDataOrders _Iorder;
        IBusinessStatusOrder _IStatusOrder;
        IBusinessEmployees _IBusinessEmployee;
        IDataTablesBusiness _IBusinessTable;

        public IBusinessStatusOrder IBusinessStatusOrder
        {
            get => _IStatusOrder;
            set => _IStatusOrder = value;
        }
        public IBusinessEmployees IBusinessEmployee
        {
            get => _IBusinessEmployee;
            set => _IBusinessEmployee = value;
        }
        public IDataTablesBusiness IBusinessTable
        {
            get => _IBusinessTable;
            set => _IBusinessTable = value;
        }


        public clsBusinessOrders
            (IDataOrders Order, IBusinessEmployees Business, IBusinessStatusOrder StatusOrder, IDataTablesBusiness Table)
        {
            Mode = enMode.Add;
            _IStatusOrder = StatusOrder;
            _IBusinessEmployee = Business;
            _IBusinessTable = Table;
            _Iorder = Order;
        }


        private async Task LoadDataAsync(DTOOrders item)
        {
            item.statusOrders = await _IStatusOrder.GetStatusOrdersByID(item.StatusOrderID);
            item.employees = await _IBusinessEmployee.GetEmployeeAsync(item.EmployerID);
            item.tables = await _IBusinessTable.LoadByID(item.TableID);
        }

        public async Task<List<DTOOrders>> GetAllOrdersAsync(int page)
        {
            var ldto = await _Iorder.GetAllOrdersAsync(page);

            foreach (var item in ldto)
            {
                await LoadDataAsync(item);
            }

            return ldto;
        }

        public async Task<DTOOrders?> GetOrderAsync(int ID)
        {
            _dtoOrders = await _Iorder.GetOrderAsync(ID);
            if (_dtoOrders == null)
            {
                Mode = enMode.Add;
                return null;
            }


            Mode = enMode.Update;
            await LoadDataAsync(_dtoOrders);
            return _dtoOrders;
        }
        public async Task<List<DTOOrders>?> GetFilterOrdersAsync
            (int Page, int TableID, int EmployeeID, int StatusOrderID)
        {
            var koko = await _Iorder.GetFilterOrder(Page, TableID, EmployeeID, StatusOrderID);
            if (koko == null)
            {
                return null;
            }

            Mode = enMode.Update;

            foreach (var item in koko)
            {
                await LoadDataAsync(item);
            }
            return koko;
        }



        private async Task<bool> Add()
        {
            if (_dtoOrderRequest == null)
                { return false; }
            if (_dtoOrders == null)
            {
                _dtoOrders = new DTOOrders();
            }
            _dtoOrders.ID = await _Iorder.Add(_dtoOrderRequest);
            if (_dtoOrders.ID != -1)
            {
                Mode = enMode.Update;
                return true;
            }
            return false;
        }

        private async Task<bool> Update()
        {
            if (_dtoOrderUpdateRequest == null)
            {
                return false;
            }
           return await _Iorder.Update(_dtoOrderUpdateRequest);
        }

        private void LoadCreated()
        {
            _dtoOrders!.TableID = _dtoOrderRequest!.TableID;
            _dtoOrders!.EmployerID = _dtoOrderRequest.EmployerID;
            _dtoOrders!.StatusOrderID = _dtoOrderRequest.StatusOrderID;
            _dtoOrders!.OrderDate = _dtoOrderRequest.OrderDate;
            _dtoOrders!.TotalAmount = _dtoOrderRequest.TotalAmount;
        }
        private void LoadUpdated()
        {
            _dtoOrders!.TableID = _dtoOrderUpdateRequest!.TableID;
            _dtoOrders!.EmployerID = _dtoOrderUpdateRequest.EmployerID;
            _dtoOrders!.StatusOrderID = _dtoOrderUpdateRequest.StatusOrderID;
            _dtoOrders!.OrderDate = _dtoOrderUpdateRequest.OrderDate;
            _dtoOrders!.TotalAmount = _dtoOrderUpdateRequest.TotalAmount;
        }

        public async Task<bool> SaveAsync()
        {
            bool result = false;
            switch (Mode)
            {
                case enMode.Add:
                    if (_dtoOrderRequest == null)
                    { return false; }
                    result = await Add();
                    LoadCreated();
                    break;
                case enMode.Update:
                    if (_dtoOrderUpdateRequest == null)
                    { return false; }
                    result = await Update();
                    LoadUpdated();
                    break;
                default:
                    result = false;
                    break;
            }

            if (result)
            {
                await LoadDataAsync(_dtoOrders);
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Iorder.Delete(ID);
        }
    }
}

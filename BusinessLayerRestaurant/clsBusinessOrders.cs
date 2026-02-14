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
    }

    public interface IWritableOrdersBusiness
    {
        Task<bool> SaveAsync();
        Task<bool> DeleteAsync(int ID);

    }

    public interface IBusinessOrders : IWritableOrdersBusiness, IRedableOrdersBusiness
    {
        DTOOrders? DTOOrders { get; set; }
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


        private async Task<bool> Add()
        {
            if (_dtoOrders == null)
                { return false; }

            _dtoOrders.ID = await _Iorder.Add(_dtoOrders);
            if (_dtoOrders.ID != -1)
            {
                Mode = enMode.Update;
                return true;
            }
            return false;
        }

        private async Task<bool> Update()
        {
            if (_dtoOrders == null)
            {
                return false;
            }
           return await _Iorder.Update(_dtoOrders);
        }

        public async Task<bool> SaveAsync()
        {
            bool result = false;
            if (_dtoOrders == null)
                { return false; }
            switch (Mode)
            {
                case enMode.Add:
                    result = await Add();
                    break;
                case enMode.Update:
                    result = await Update();
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

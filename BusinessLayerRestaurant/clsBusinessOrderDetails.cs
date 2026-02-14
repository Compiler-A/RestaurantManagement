using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IReadableOrderDetailsBusiness
    {
        Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page);
        Task<DTOOrderDetails?> GetOrderDetailsAsync(int page);
    }

    public interface IWritableOrderDetailsBusiness
    {
        Task<bool> SaveAsync();
        Task<bool> DeleteAsync(int ID);
    }

    public interface IBusinessOrderDetails : IWritableOrderDetailsBusiness, IReadableOrderDetailsBusiness
    {
        DTOOrderDetails? dtoOrderDetail { get; set; }
        IBusinessMenuItems IBusinessItem {  get; set; }
        IBusinessOrders IBusinessOrder { get; set; }
    }

    public class clsBusinessOrderDetails : IBusinessOrderDetails
    {
        public enum enMode
        {
            Add, Update
        }
        public enMode Mode {  get; set; } = enMode.Add;

        private DTOOrderDetails? _dtoOrderDetail;
        public DTOOrderDetails? dtoOrderDetail
        {
            get => _dtoOrderDetail;
            set => _dtoOrderDetail = value;
        }

        private IBusinessMenuItems _IBusinessItem;
        private IBusinessOrders _IBusinessOrder;
        private IDataOrderDetails _IDataOrderDetails;
        public IBusinessMenuItems IBusinessItem
        {
            get => _IBusinessItem;
            set => _IBusinessItem = value;
        }
        public IBusinessOrders IBusinessOrder
        {
            get => _IBusinessOrder;
            set => _IBusinessOrder = value;
        }

        public clsBusinessOrderDetails
            (IDataOrderDetails iDataOrderDetails, IBusinessOrders iBusinessOrders, IBusinessMenuItems iBusinessMenuItems)
        {
            _IBusinessItem = iBusinessMenuItems;
            _IBusinessOrder = iBusinessOrders;
            _IDataOrderDetails = iDataOrderDetails;
        }


        private async Task LoadDataAsync(DTOOrderDetails item)
        {
            item.Order = await _IBusinessOrder.GetOrderAsync(item.OrderID);
            item.Item = await _IBusinessItem.GetMenuItemByIdAsync(item.ItemID);
        }

        public async Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page)
        {
            var ldto = await _IDataOrderDetails.GetAllOrderDetailsAsync(page);

            foreach (var item in ldto)
            {
                await LoadDataAsync(item);
            }

            return ldto;
        }

        public async Task<DTOOrderDetails?> GetOrderDetailsAsync(int ID)
        {
            _dtoOrderDetail = await _IDataOrderDetails.GetOrderDetailAsync(ID);
            if (_dtoOrderDetail == null)
            {
                Mode = enMode.Add;
                return null;
            }


            Mode = enMode.Update;
            await LoadDataAsync(_dtoOrderDetail);
            return _dtoOrderDetail;
        }


        private async Task<bool> Add()
        {
            if (_dtoOrderDetail == null)
            { return false; }

            _dtoOrderDetail.ID = await _IDataOrderDetails.AddAsync(_dtoOrderDetail);
            if (_dtoOrderDetail.ID != -1)
            {
                Mode = enMode.Update;
                return true;
            }
            return false;
        }

        private async Task<bool> Update()
        {
            if (_dtoOrderDetail == null)
            {
                return false;
            }
            return await _IDataOrderDetails.UpdateAsync(_dtoOrderDetail);
        }

        public async Task<bool> SaveAsync()
        {
            bool result = false;
            if (_dtoOrderDetail == null)
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
                await LoadDataAsync(_dtoOrderDetail);
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IDataOrderDetails.DeleteAsync(ID);
        }
    }
}

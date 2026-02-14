using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public interface IReadableStatusOrderBusiness
    {
        Task<List<DTOStatusOrders>> GetAllStatusOrders(int page);
        Task<DTOStatusOrders?> GetStatusOrdersByID(int ID);
    }

    public interface IWritableStatusOrderBusiness
    {
        Task<bool> Save();
        Task<bool> Delete(int ID);
    }

    public interface IBusinessStatusOrder : IReadableStatusOrderBusiness , IWritableStatusOrderBusiness
    {
        DTOStatusOrders? StatusOrders { get; set; }
    }


    public class clsBusinessStatusOrders : IBusinessStatusOrder
    {
        public enum enMode
        {
            Add,
            Update
        }

        public enMode Mode { get; set; } = enMode.Add;

        private readonly IDataStatusOrders _dataStatusOrder;

        private DTOStatusOrders? _statusOrders;

        public DTOStatusOrders? StatusOrders
        {
            get => _statusOrders;
            set => _statusOrders = value;
        }

        // ✅ DI صحيح
        public clsBusinessStatusOrders(IDataStatusOrders dataStatusOrder)
        {
            _dataStatusOrder = dataStatusOrder;
        }

        // ==================== READ ====================

        public async Task<List<DTOStatusOrders>> GetAllStatusOrders(int page)
        {
            return await _dataStatusOrder.GetAllStatusOrders(page);
        }

        public async Task<DTOStatusOrders?> GetStatusOrdersByID(int ID)
        {
            var dto = await _dataStatusOrder.GetStatusOrderByID(ID);

            if (dto == null)
                return null;

            _statusOrders = dto;
            Mode = enMode.Update;

            return dto;
        }

        // ==================== WRITE (PROTECTED CORE) ====================

        protected async Task<bool> _Add()
        {
            if (_statusOrders == null)
                throw new InvalidOperationException("StatusOrders is NULL before Add.");

            int newID = await _dataStatusOrder.AddStatusOrder(_statusOrders);

            if (newID <= 0)
                return false;

            _statusOrders.idStatusOrder = newID;
            Mode = enMode.Update;

            return true;
        }

        protected async Task<bool> _Update()
        {
            if (_statusOrders == null)
                throw new InvalidOperationException("StatusOrders is NULL before Update.");

            return await _dataStatusOrder.UpdateStatusOrder(_statusOrders);
        }

        // ==================== PUBLIC API ====================

        public async Task<bool> Save()
        {
            if (_statusOrders == null)
                throw new InvalidOperationException("StatusOrders must be set before calling Save().");

            return Mode switch
            {
                enMode.Add => await _Add(),
                enMode.Update => await _Update(),
                _ => false
            };
        }

        public async Task<bool> Delete(int ID)
        {
            if (ID <= 0)
                return false;

            return await _dataStatusOrder.DeleteStatusOrder(ID);
        }
    }

}

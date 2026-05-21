using ContractsLayerRestaurant.DTORequest.Orders;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{
    public class OrdersService : IOrdersService
    {
        private IOrdersServiceContainer _Interface;
        private IOrdersServiceWriter _IWrite;
        private IOrdersServiceReader _Read;

        public OrdersService(
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

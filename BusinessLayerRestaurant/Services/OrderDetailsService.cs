using ContractsLayerRestaurant.DTORequest.OrderDetails;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private IOrderDetailsServiceContainer _Interface;
        private IOrderDetailsServiceWriter _IWrite;
        private IOrderDetailsServiceReader _Read;

        public OrderDetailsService(
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

        public Task<List<OrderDetail>> GetAllAsync(int page)
        {
            return _Read.GetAllAsync(page);
        }

        public async Task<OrderDetail?> GetAsync(int ID)
        {
            return await _Read.GetAsync(ID);
        }

        public async Task<List<OrderDetail>> GetAllByOrderIDAsync(int OrderID)
        {
            return await _Read.GetAllByOrderIDAsync(OrderID);
        }
        public async Task<OrderDetail?> CreateAsync(DTOOrderDetailsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<OrderDetail?> UpdateAsync(DTOOrderDetailsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

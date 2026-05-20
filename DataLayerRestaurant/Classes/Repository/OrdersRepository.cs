using ContractsLayerRestaurant.DTORequest.Orders;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class OrdersRepository : IOrdersRepository
    {
        IOrdersRepositoryWriter _Write;
        IOrdersRepositoryReader _Read;

        public OrdersRepository(IOrdersRepositoryWriter write, IOrdersRepositoryReader read)
        {
            _Write = write;
            _Read = read;
        }

        public async Task<List<Order>> GetAllDataAsync(List<int> Ids)
        {
            return await _Read.GetAllDataAsync(Ids);
        }

        public async Task<List<Order>> GetAllDataAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }
        public async Task<Order?> GetDataAsync(int ID)
        {
            return await _Read.GetDataAsync(ID);
        }
        public async Task<List<Order>?> GetFilterDataAsync(DTOOrderFilterRequest Request)
        {
            return await _Read.GetFilterDataAsync(Request);
        }

        public async Task<Order?> CreateDataAsync(DTOOrderCRequest order)
        {
            return await _Write.CreateDataAsync(order);
        }
        public async Task<Order?> UpdateDataAsync(DTOOrderURequest order)
        {
            return await _Write.UpdateDataAsync(order);
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _Write.DeleteDataAsync(ID);
        }
    }
}

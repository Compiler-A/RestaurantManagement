using ContractsLayerRestaurant.DTORequest.OrderDetails;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        IOrderDetailsRepositoryReader _Read;
        IOrderDetailsRepositoryWriter _Write;
        public OrderDetailsRepository(IOrderDetailsRepositoryReader Read, IOrderDetailsRepositoryWriter Write)
        {
            _Read = Read;
            _Write = Write;
        }

        public async Task<List<OrderDetail>> GetAllDataByOrderIdsAsync(List<int> Ids)
        {
            return await _Read.GetAllDataByOrderIdsAsync(Ids);
        }
        public async Task<List<OrderDetail>> GetAllDataAsync(List<int> Ids)
        {
            return await _Read.GetAllDataAsync(Ids);
        }

        public async Task<List<OrderDetail>> GetAllDataByOrderIDAsync(int orderID)
        {
            return await _Read.GetAllDataByOrderIDAsync(orderID);
        }
        public async Task<List<OrderDetail>> GetAllDataAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }

        public async Task<OrderDetail?> GetDataAsync(int ID)
        {
            return await _Read.GetDataAsync(ID);
        }

        public async Task<OrderDetail?> CreateDataAsync(DTOOrderDetailsCRequest Request)
        {
            return await _Write.CreateDataAsync(Request);
        }

        public async Task<OrderDetail?> UpdateDataAsync(DTOOrderDetailsURequest Request)
        {
            return await _Write.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await (_Write.DeleteDataAsync(ID));
        }
    }
}

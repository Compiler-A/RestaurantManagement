using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class StatusOrdersRepository : IStatusOrdersRepository
    {
        IStatusOrdersRepositoryReader _Read;
        IStatusOrdersRepositoryWriter _Write;
        public StatusOrdersRepository(IStatusOrdersRepositoryReader Read, IStatusOrdersRepositoryWriter Write)
        {
            _Read = Read;
            _Write = Write;
        }

        public async Task<List<StatusOrder>> GetAllDataAsync(List<int> Ids)
        {
            return await _Read.GetAllDataAsync(Ids);
        }

        public async Task<StatusOrder?> GetDataAsync(int id)
        {
            return await _Read.GetDataAsync(id);
        }
        public async Task<List<StatusOrder>> GetAllDataAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }
        public async Task<StatusOrder?> CreateDataAsync(DTOStatusOrdersCRequest Request)
        {
            return await _Write.CreateDataAsync(Request);
        }
        public async Task<StatusOrder?> UpdateDataAsync(DTOStatusOrdersURequest Request)
        {
            return await _Write.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteDataAsync(int id)
        {
            return await _Write.DeleteDataAsync(id);
        }
    }
}

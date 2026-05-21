using ContractsLayerRestaurant.DTORequest.StatusOrders;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{

    public class StatusOrdersService : IStatusOrdersService
    {
        private IStatusOrdersServiceReader _IRead;
        private IStatusOrdersServiceWriter _IWrite;

        public StatusOrdersService(IStatusOrdersServiceReader read, IStatusOrdersServiceWriter write)
        {
            _IRead = read;
            _IWrite = write;
        }


        public async Task<List<StatusOrder>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<StatusOrder?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<StatusOrder?> CreateAsync(DTOStatusOrdersCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<StatusOrder?> UpdateAsync(DTOStatusOrdersURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }

    }
}

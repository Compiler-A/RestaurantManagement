using ContractsLayerRestaurant.DTORequest.StatusMenus;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{

    public class StatusMenusService : IStatusMenusService
    {
        IStatusMenusServiceReader _IRead;
        IStatusMenusServiceWriter _IWrite;

        public StatusMenusService(IStatusMenusServiceWriter Write, IStatusMenusServiceReader Read)
        {
            _IRead = Read;
            _IWrite = Write;
        }

        public async Task<List<StatusMenu>> GetAllAsync(int Page)
        {
            return await _IRead.GetAllAsync(Page);
        }
        public async Task<StatusMenu?> GetAsync(int Page)
        {
            return await _IRead.GetAsync(Page);
        }

        public async Task<StatusMenu?> CreateAsync(DTOStatusMenusCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }

        public async Task<StatusMenu?> UpdateAsync(DTOStatusMenusURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }
        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

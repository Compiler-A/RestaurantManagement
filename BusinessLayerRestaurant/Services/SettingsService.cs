using ContractsLayerRestaurant.DTORequest.Settings;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{

    public class SettingsService : ISettingsService
    {
        private ISettingsServiceWriter _IWrite;
        private ISettingsServiceReader _IRead;

        public SettingsService(
            ISettingsServiceWriter Write,
            ISettingsServiceReader read)
        {
            _IWrite = Write;
            _IRead = read;
        }


        public async Task<List<Setting>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<Setting?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<Setting?> CreateAsync(DTOSettingsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<Setting?> UpdateAsync(DTOSettingsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

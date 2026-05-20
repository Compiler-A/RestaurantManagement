using ContractsLayerRestaurant.DTORequest.Settings;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ISettingsRepositoryWriter _Write;
        private readonly ISettingsRepositoryReader _Read;

        public SettingsRepository(ISettingsRepositoryWriter write, ISettingsRepositoryReader read)
        {
            _Write = write;
            _Read = read;
        }

        public async Task<List<Setting>> GetAllDataAsync(List<int> Ids)
        {
            return await _Read.GetAllDataAsync(Ids);
        }

        public async Task<List<Setting>> GetAllDataAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }

        public async Task<Setting?> GetDataAsync(int ID)
        {
            return await _Read.GetDataAsync(ID);
        }

        public async Task<Setting?> CreateDataAsync(DTOSettingsCRequest DTO)
        {

            return await _Write.CreateDataAsync(DTO);
        }

        public async Task<Setting?> UpdateDataAsync(DTOSettingsURequest DTO)
        {

            return await _Write.UpdateDataAsync(DTO);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _Write.DeleteDataAsync(ID);
        }
    }
}

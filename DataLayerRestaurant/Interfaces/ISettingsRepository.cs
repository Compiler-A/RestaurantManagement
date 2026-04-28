using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.Settings;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Interfaces
{
    public interface ISettingsRepositoryReader : IReadableDataBase<Setting>
    { }
    public interface ISettingsRepositoryWriter : IWritableDataBase<Setting,DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface ISettingsRepositoryReadable
    {
        Task<List<Setting>> GetAllSettingsAsync(int page);
        Task<Setting?> GetSettingAsync(int ID);
    }
    public interface ISettingsRepositoryWritable
    {
        Task<Setting?> AddSettingAsync(DTOSettingsCRequest setting);
        Task<Setting?> UpdateSettingAsync(DTOSettingsURequest setting);
        Task<bool> DeleteSettingAsync(int ID);
    }

    public interface ISettingsRepository : ISettingsRepositoryReadable, ISettingsRepositoryWritable
    { }
}

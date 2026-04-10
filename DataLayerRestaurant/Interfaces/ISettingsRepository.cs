using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.Settings;


namespace DataLayerRestaurant.Interfaces
{
    public interface ISettingsRepositoryReader : IReadableDataBase<DTOSettings>
    { }
    public interface ISettingsRepositoryWriter : IWritableDataBase<DTOSettings,DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface ISettingsRepositoryReadable
    {
        Task<List<DTOSettings>> GetAllSettingsAsync(int page);
        Task<DTOSettings?> GetSettingAsync(int ID);
    }
    public interface ISettingsRepositoryWritable
    {
        Task<DTOSettings?> AddSettingAsync(DTOSettingsCRequest setting);
        Task<DTOSettings?> UpdateSettingAsync(DTOSettingsURequest setting);
        Task<bool> DeleteSettingAsync(int ID);
    }

    public interface ISettingsRepository : ISettingsRepositoryReadable, ISettingsRepositoryWritable
    { }
}

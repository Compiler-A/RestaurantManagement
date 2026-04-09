using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.Settings;


namespace DataLayerRestaurant.Interfaces
{
    public interface IReadableDSettings : IReadableDataBase<DTOSettings>
    { }
    public interface IWritableDSettings : IWritableDataBase<DTOSettings,DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface IReadableDataSettings
    {
        Task<List<DTOSettings>> GetAllSettingsAsync(int page);
        Task<DTOSettings?> GetSettingAsync(int ID);
    }
    public interface IWritableDataSettings
    {
        Task<DTOSettings?> AddSettingAsync(DTOSettingsCRequest setting);
        Task<DTOSettings?> UpdateSettingAsync(DTOSettingsURequest setting);
        Task<bool> DeleteSettingAsync(int ID);
    }

    public interface IDataSettings : IReadableDataSettings, IWritableDataSettings
    { }
}

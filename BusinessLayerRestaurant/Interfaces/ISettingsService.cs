using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Settings;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface ISettingsServiceContainer : IInterfaceBase<ISettingsRepository>
    { }

    public interface ISettingsServiceReader : IReadableBusinessBase<DTOSettings>
    { }
    public interface ISettingsServiceWriter : IWritableBusinessBase<DTOSettings, DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface ISettingsServiceReadable
    {
        Task<List<DTOSettings>> GetAllSettingsAsync(int page);
        Task<DTOSettings?> GetSettingAsync(int ID);
    }
    public interface ISettingsServiceWritable
    {
        Task<DTOSettings?> AddSettingAsync(DTOSettingsCRequest setting);
        Task<DTOSettings?> UpdateSettingAsync(DTOSettingsURequest setting);
        Task<bool> DeleteSettingAsync(int ID);
    }
    public interface ICRUDSettingsService : ISettingsServiceWritable, ISettingsServiceReadable
    { }
    public interface ISettingsService : ICRUDSettingsService
    {
    }
}

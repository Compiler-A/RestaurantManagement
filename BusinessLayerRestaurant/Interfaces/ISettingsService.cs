using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Settings;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface ISettingsServiceContainer : IServiceContainer<ISettingsRepository>
    { }

    public interface ISettingsServiceReader : IServiceReader<Setting>
    { }
    public interface ISettingsServiceWriter : IServiceWriter<Setting, DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface ISettingsServiceReadable
    {
        Task<List<Setting>> GetAllSettingsAsync(int page);
        Task<Setting?> GetSettingAsync(int ID);
    }
    public interface ISettingsServiceWritable
    {
        Task<Setting?> AddSettingAsync(DTOSettingsCRequest setting);
        Task<Setting?> UpdateSettingAsync(DTOSettingsURequest setting);
        Task<bool> DeleteSettingAsync(int ID);
    }
    public interface ICRUDSettingsService : ISettingsServiceWritable, ISettingsServiceReadable
    { }
    public interface ISettingsService : ICRUDSettingsService
    {
    }
}

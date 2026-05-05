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

    public interface ICRUDSettingsService : ISettingsServiceWriter, ISettingsServiceReader
    { }
    public interface ISettingsService : ICRUDSettingsService
    {
    }
}

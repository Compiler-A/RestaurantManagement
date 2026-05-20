using ContractsLayerRestaurant.DTORequest.Settings;
using DomainLayer.Entities;
using ContractsLayerRestaurant.Interfaces.Repositories;

namespace ContractsLayerRestaurant.Interfaces.Services
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

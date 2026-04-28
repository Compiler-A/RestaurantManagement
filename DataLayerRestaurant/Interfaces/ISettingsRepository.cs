using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.Settings;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Interfaces
{
    public interface ISettingsRepositoryReader : IRepositoryReader<Setting>
    { }
    public interface ISettingsRepositoryWriter : IRepositoryWriter<Setting,DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface ISettingsRepository : ISettingsRepositoryReader, ISettingsRepositoryWriter
    { }
}

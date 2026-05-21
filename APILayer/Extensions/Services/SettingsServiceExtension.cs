using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddSettingsServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<ISettingsRepositoryWriter, SettingsRepositoryWriterEF>();
                Services.AddScoped<ISettingsRepositoryReader, SettingsRepositoryReaderEF>();
            }
            else
            {
                Services.AddScoped<ISettingsRepositoryWriter, SettingsRepositoryWriter>();
                Services.AddScoped<ISettingsRepositoryReader, SettingsRepositoryReader>();
            }
            Services.AddScoped<ISettingsRepository, SettingsRepository>();
            Services.AddScoped<ISettingsServiceReader, SettingsReader>();
            Services.AddScoped<ISettingsServiceWriter, SettingsWriter>();
            Services.AddScoped<ISettingsServiceContainer, SettingsContainer>();
            Services.AddScoped<ISettingsService, SettingsService>();
            return Services;
        }
    }
}

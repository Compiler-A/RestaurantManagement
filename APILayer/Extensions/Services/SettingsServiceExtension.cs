using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static class SettingsServiceExtension
    {
        public static IServiceCollection AddSettingsServices(this IServiceCollection Services)
        {
            Services.AddScoped<ISettingsRepository, SettingsRepository>();
            Services.AddScoped<ISettingsRepositoryWriter, SettingsRepositoryWriter>();
            Services.AddScoped<ISettingsRepositoryReader, SettingsRepositoryReader>();
            Services.AddScoped<ISettingsServiceReader, SettingsReader>();
            Services.AddScoped<ISettingsServiceWriter, SettingsWriter>();
            Services.AddScoped<ISettingsServiceContainer, SettingsContainer>();
            Services.AddScoped<ISettingsService, SettingsService>();
            return Services;
        }
    }
}

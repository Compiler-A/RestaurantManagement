using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;

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

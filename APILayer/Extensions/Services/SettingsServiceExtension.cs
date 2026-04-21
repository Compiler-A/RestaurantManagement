using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions.Services
{
    public static class SettingsServiceExtension
    {
        public static IServiceCollection AddSettingsServices(this IServiceCollection Services)
        {
            Services.AddScoped<ISettingsRepository, clsSettingsRepository>();
            Services.AddScoped<ISettingsRepositoryWriter, clsSettingsRepositoryWriter>();
            Services.AddScoped<ISettingsRepositoryReader, clsSettingsRepositoryReader>();
            Services.AddScoped<ISettingsServiceReader, clsSettingsReader>();
            Services.AddScoped<ISettingsServiceWriter, clsSettingsWriter>();
            Services.AddScoped<ISettingsServiceContainer, clsSettingsContainer>();
            Services.AddScoped<ISettingsService, clsSettingsService>();
            return Services;
        }
    }
}

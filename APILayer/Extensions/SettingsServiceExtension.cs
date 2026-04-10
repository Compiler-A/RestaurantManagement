using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class SettingsServiceExtension
    {
        public static IServiceCollection AddSettingsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IDataSettings, clsDataSettings>();
            Services.AddScoped<IWritableDSettings, DataLayerRestaurant.Classes.clsSettingsWriter>();
            Services.AddScoped<IReadableDSettings, DataLayerRestaurant.Classes.clsSettingsReader>();
            Services.AddScoped<ISettingsServiceReader, BusinessLayerRestaurant.Classes.clsSettingsReader>();
            Services.AddScoped<ISettingsServiceWriter, BusinessLayerRestaurant.Classes.clsSettingsWriter>();
            Services.AddScoped<ISettingsServiceContainer, clsSettingsContainer>();
            Services.AddScoped<ISettingsService, clsSettingsService>();
            return Services;
        }
    }
}

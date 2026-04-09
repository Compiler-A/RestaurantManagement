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
            Services.AddScoped<IReadableBSettings, BusinessLayerRestaurant.Classes.clsSettingsReader>();
            Services.AddScoped<IWritableBSettings, BusinessLayerRestaurant.Classes.clsSettingsWriter>();
            Services.AddScoped<IInterfaceBSettings, clsSettingsRepositoryBridge>();
            Services.AddScoped<IBusinessSettings, clsBusinessSettings>();
            return Services;
        }
    }
}

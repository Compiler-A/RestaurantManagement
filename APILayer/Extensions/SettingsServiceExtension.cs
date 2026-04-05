using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class SettingsServiceExtension
    {
        public static IServiceCollection AddSettingsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IDataSettings, clsDataSettings>();
            Services.AddScoped<IWritableDSettings, DataLayerRestaurant.clsSettingsWriter>();
            Services.AddScoped<IReadableDSettings, DataLayerRestaurant.clsSettingsReader>();
            Services.AddScoped<IReadableBSettings, BusinessLayerRestaurant.clsSettingsReader>();
            Services.AddScoped<IWritableBSettings, BusinessLayerRestaurant.clsSettingsWriter>();
            Services.AddScoped<IDTOBSettings, clsSettingsDtoContainer>();
            Services.AddScoped<IInterfaceBSettings, clsSettingsRepositoryBridge>();
            Services.AddScoped<IBusinessSettings, clsBusinessSettings>();
            return Services;
        }
    }
}

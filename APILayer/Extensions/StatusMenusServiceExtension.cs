using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class StatusMenusServiceExtension
    {
        public static IServiceCollection AddStatusMenusServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDStatusMenus, DataLayerRestaurant.Classes.clsStatusMenusReader>();
            Services.AddScoped<IWritableDStatusMenus, DataLayerRestaurant.Classes.clsStatusMenusWriter>();
            Services.AddScoped<IDataStatusMenus, clsDataStatusMenus>();
            Services.AddScoped<IWritableBStatusMenus, BusinessLayerRestaurant.Classes.clsStatusMenusWriter>();
            Services.AddScoped<IReadableBStatusMenus, BusinessLayerRestaurant.Classes.clsStatusMenusReader>();
            Services.AddScoped<IInterfaceBStatusMenus, clsStatusMenusRepositoryBridge>();
            Services.AddScoped<IBusinessStatusMenus, clsBusinessStatusMenus>();
            return Services;
        }
    }
}

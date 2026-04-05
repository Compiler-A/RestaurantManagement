using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class StatusMenusServiceExtension
    {
        public static IServiceCollection AddStatusMenusServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDStatusMenus, DataLayerRestaurant.clsStatusMenusReader>();
            Services.AddScoped<IWritableDStatusMenus, DataLayerRestaurant.clsStatusMenusWriter>();
            Services.AddScoped<IDataStatusMenus, clsDataStatusMenus>();
            Services.AddScoped<IWritableBStatusMenus, BusinessLayerRestaurant.clsStatusMenusWriter>();
            Services.AddScoped<IReadableBStatusMenus, BusinessLayerRestaurant.clsStatusMenusReader>();
            Services.AddScoped<IDTOBStatusMenus, clsStatusMenusDtoContainer>();
            Services.AddScoped<IInterfaceBStatusMenus, clsStatusMenusRepositoryBridge>();
            Services.AddScoped<IBusinessStatusMenus, clsBusinessStatusMenus>();
            return Services;
        }
    }
}

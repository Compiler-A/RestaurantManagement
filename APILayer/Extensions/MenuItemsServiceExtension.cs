using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class MenuItemsServiceExtension
    {
        public static IServiceCollection AddMenuItemsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDMenuItems, DataLayerRestaurant.clsMenuItemsReader>();
            Services.AddScoped<IWritableDMenuItems, DataLayerRestaurant.clsMenuItemsWriter>();
            Services.AddScoped<IDataMenuItems, clsDataMenuItems>();
            Services.AddScoped<ICompositionBMenuItems, clsTypeItemLoader>();
            Services.AddScoped<ICompositionBMenuItems, clsStatusMenuLoader>();
            Services.AddScoped<IDTOBMenuItems, clsMenuItemsDtoContainer>();
            Services.AddScoped<IInterfaceBMenuItems, clsMenuItemsRepositoryBridge>();
            Services.AddScoped<IReadableBMenuItems, BusinessLayerRestaurant.clsMenuItemsReader>();
            Services.AddScoped<IWritableBMenuItems, BusinessLayerRestaurant.clsMenuItemsWriter>();
            Services.AddScoped<IBusinessMenuItems, clsBusinessMenuItem>();
            return Services;
        }
    }
}

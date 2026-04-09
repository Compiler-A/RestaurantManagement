using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class MenuItemsServiceExtension
    {
        public static IServiceCollection AddMenuItemsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDMenuItems, DataLayerRestaurant.Classes.clsMenuItemsReader>();
            Services.AddScoped<IWritableDMenuItems, DataLayerRestaurant.Classes.clsMenuItemsWriter>();
            Services.AddScoped<IDataMenuItems, clsDataMenuItems>();
            Services.AddScoped<ICompositionBMenuItems, clsTypeItemLoader>();
            Services.AddScoped<ICompositionBMenuItems, clsStatusMenuLoader>();
            Services.AddScoped<IInterfaceBMenuItems, clsMenuItemsRepositoryBridge>();
            Services.AddScoped<IReadableBMenuItems, BusinessLayerRestaurant.Classes.clsMenuItemsReader>();
            Services.AddScoped<IWritableBMenuItems, BusinessLayerRestaurant.Classes.clsMenuItemsWriter>();
            Services.AddScoped<IBusinessMenuItems, clsBusinessMenuItem>();
            return Services;
        }
    }
}

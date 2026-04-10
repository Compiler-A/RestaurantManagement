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
            Services.AddScoped<IMenuItemsServiceComposition, clsTypeItemLoader>();
            Services.AddScoped<IMenuItemsServiceComposition, clsStatusMenuLoader>();
            Services.AddScoped<IMenuItemsServiceContainer, clsMenuItemsContainer>();
            Services.AddScoped<IMenuItemsServiceReader, BusinessLayerRestaurant.Classes.clsMenuItemsReader>();
            Services.AddScoped<IMenuItemsServiceWriter, BusinessLayerRestaurant.Classes.clsMenuItemsWriter>();
            Services.AddScoped<IMenuItemsService, clsMenuItemService>();
            return Services;
        }
    }
}

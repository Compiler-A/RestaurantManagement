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
            Services.AddScoped<IMenuItemsRepositoryReader, clsMenuItemsRepositoryReader>();
            Services.AddScoped<IMenuItemsRepositoryWriter, clsMenuItemsRepositoryWriter>();
            Services.AddScoped<IMenuItemsRepository, clsMenuItemsRepository>();
            Services.AddScoped<IMenuItemsServiceComposition, clsTypeItemLoader>();
            Services.AddScoped<IMenuItemsServiceComposition, clsStatusMenuLoader>();
            Services.AddScoped<IMenuItemsServiceContainer, clsMenuItemsContainer>();
            Services.AddScoped<IMenuItemsServiceReader, clsMenuItemsReader>();
            Services.AddScoped<IMenuItemsServiceWriter, clsMenuItemsWriter>();
            Services.AddScoped<IMenuItemsService, clsMenuItemService>();
            return Services;
        }
    }
}

using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;
using RestaurantDataLayer;
using DomainLayer.Entities;

namespace APILayer.Extensions.Services
{
    public static class MenuItemsServiceExtension
    {
        public static IServiceCollection AddMenuItemsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IRepositoryBatchsLoader<MenuItem>, clsTypeItemBatchLoader>();
            Services.AddScoped<IRepositoryBatchsLoader<MenuItem>, clsStatusMenuBatchLoader>();
            Services.AddScoped<IMenuItemsRepositoryLoader, clsMenuItemsRepositoryLoader>();
            Services.AddScoped<IMenuItemsRepositoryReader, clsMenuItemsRepositoryReader>();
            Services.AddScoped<IMenuItemsRepositoryWriter, clsMenuItemsRepositoryWriter>();
            Services.AddScoped<IMenuItemsRepository, clsMenuItemsRepository>();

            Services.AddScoped<IMenuItemsServiceContainer, clsMenuItemsContainer>();
            Services.AddScoped<IMenuItemsServiceReader, clsMenuItemsReader>();
            Services.AddScoped<IMenuItemsServiceWriter, clsMenuItemsWriter>();
            Services.AddScoped<IMenuItemsService, clsMenuItemService>();
            return Services;
        }
    }
}

using ContractsLayerRestaurant.Interfaces.Services;
using BusinessLayerRestaurant.Classes;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.EF;
using DataLayerRestaurant.Classes.Repository;


namespace APILayer.Extensions.Services
{
    public static class MenuItemsServiceExtension
    {
        public static IServiceCollection AddMenuItemsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IMenuItemsRepositoryReader, MenuItemsRepositoryReader>();
            Services.AddScoped<IMenuItemsRepositoryWriter, MenuItemsRepositoryWriter>();
            Services.AddScoped<IMenuItemsRepository, MenuItemsRepository>();

            Services.AddScoped<IMenuItemsServiceContainer, MenuItemsContainer>();
            Services.AddScoped<IMenuItemsServiceReader, MenuItemsReader>();
            Services.AddScoped<IMenuItemsServiceWriter, MenuItemsWriter>();
            Services.AddScoped<IMenuItemsService, MenuItemsService>();
            return Services;
        }
    }
}

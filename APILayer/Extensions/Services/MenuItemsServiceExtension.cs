using ContractsLayerRestaurant.Interfaces.Services;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.EF;
using DataLayerRestaurant.Classes.Repository;


namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddMenuItemsServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IMenuItemsRepositoryReader, MenuItemsRepositoryReaderEF>();
                Services.AddScoped<IMenuItemsRepositoryWriter, MenuItemsRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<IMenuItemsRepositoryReader, MenuItemsRepositoryReader>();
                Services.AddScoped<IMenuItemsRepositoryWriter, MenuItemsRepositoryWriter>();
            }
            
            Services.AddScoped<IMenuItemsRepository, MenuItemsRepository>();

            Services.AddScoped<IMenuItemsServiceContainer, MenuItemsContainer>();
            Services.AddScoped<IMenuItemsServiceReader, MenuItemsReader>();
            Services.AddScoped<IMenuItemsServiceWriter, MenuItemsWriter>();
            Services.AddScoped<IMenuItemsService, MenuItemsService>();
            return Services;
        }
    }
}

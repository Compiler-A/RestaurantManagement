using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions.Services
{
    public static class StatusOrdersServiceExtension
    {
        public static IServiceCollection AddStatusOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IStatusOrdersRepositoryReader, clsStatusOrdersRepositoryReader>();
            Services.AddScoped<IStatusOrdersRepositoryWriter, clsStatusOrdersRepositoryWriter>();
            Services.AddScoped<IStatusOrdersRepository, clsStatusOrdersRepository>();
            Services.AddScoped<IStatusOrdersServiceReader, clsStatusOrdersReader>();
            Services.AddScoped<IWritableBStatusOrders, clsStatusOrdersWriter>();
            Services.AddScoped<IStatusOrdersServiceContainer, clsStatusOrdersContainer>();
            Services.AddScoped<IStatusOrdersService, clsStatusOrdersService>();
            return Services;
        }
    }
}

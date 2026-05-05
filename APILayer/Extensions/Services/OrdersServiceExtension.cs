using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions.Services
{
    public static class OrdersServiceExtension
    {
        public static IServiceCollection AddOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IOrderDetailBatchLoader, clsOrderDetailBatchLoader>();

            Services.AddScoped<IOrdersRepositoryWriter, clsOrdersRepositoryWriter>();
            Services.AddScoped<IOrdersRepositoryReader, clsOrdersRepositoryReader>();
            Services.AddScoped<IOrdersRepository, clsOrdersRepository>();
            Services.AddScoped<IOrdersServiceWriter, clsOrdersWriter>();
            Services.AddScoped<IOrdersServiceReader, clsOrdersReader>();
            Services.AddScoped<IOrdersServiceContainer, clsOrdersContainer>();
            Services.AddScoped<IOrdersService, clsOrdersService>();
            return Services;
        }
    }
}

using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;

namespace APILayer.Extensions.Services
{
    public static class OrdersServiceExtension
    {
        public static IServiceCollection AddOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IOrderDetailBatchLoader, OrderDetailBatchLoader>();

            Services.AddScoped<IOrdersRepositoryWriter, OrdersRepositoryWriter>();
            Services.AddScoped<IOrdersRepositoryReader, OrdersRepositoryReader>();
            Services.AddScoped<IOrdersRepository, OrdersRepository>();
            Services.AddScoped<IOrdersServiceWriter, OrdersWriter>();
            Services.AddScoped<IOrdersServiceReader, OrdersReader>();
            Services.AddScoped<IOrdersServiceContainer, OrdersContainer>();
            Services.AddScoped<IOrdersService, OrdersService>();
            return Services;
        }
    }
}

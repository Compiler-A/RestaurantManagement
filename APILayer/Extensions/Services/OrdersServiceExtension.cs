using ContractsLayerRestaurant.Interfaces.Services;
using BusinessLayerRestaurant.Classes;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static class OrdersServiceExtension
    {
        public static IServiceCollection AddOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IOrderDetailBatchLoader, DataLayerRestaurant.Classes.SQL.OrderDetailBatchLoader>();

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

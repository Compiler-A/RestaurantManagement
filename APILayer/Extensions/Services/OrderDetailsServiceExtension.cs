using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;
using RestaurantDataLayer;
using DomainLayer.Entities;

namespace APILayer.Extensions.Services
{
    public static class OrderDetailsServiceExtension
    {
        public static IServiceCollection AddOrderDetailsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IRepositoryBatchsLoader<OrderDetail>, clsOrderBatchLoader>();
            Services.AddScoped<IRepositoryBatchsLoader<OrderDetail>, clsMenuItemBatchLoader>();
            Services.AddScoped<IOrderDetailsRepositoryLoader, clsOrderDetailsRepositoryLoader>();
            Services.AddScoped<IOrderDetailsRepositoryReader, clsOrderDetailsRepositoryReader>();
            Services.AddScoped<IOrderDetailsRepositoryWriter, clsOrderDetailsRepositoryWriter>();
            Services.AddScoped<IOrderDetailsRepository, clsOrderDetailsRepository>();
            Services.AddScoped<IOrderDetailsServiceReader, clsOrderDetailsReader>();
            Services.AddScoped<IOrderDetailsServiceWriter, clsOrderDetailsWriter>();
            Services.AddScoped<IOrderDetailsServiceContainer, clsOrderDetailsContainer>();
            Services.AddScoped<IOrderDetailsService, clsOrderDetailsService>();
            return Services;
        }
    }
}

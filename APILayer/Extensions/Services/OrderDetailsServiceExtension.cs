using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;


namespace APILayer.Extensions.Services
{
    public static class OrderDetailsServiceExtension
    {
        public static IServiceCollection AddOrderDetailsServices(this IServiceCollection Services)
        {

            Services.AddScoped<IOrderDetailsRepositoryReader, OrderDetailsRepositoryReader>();
            Services.AddScoped<IOrderDetailsRepositoryWriter, OrderDetailsRepositoryWriter>();
            Services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            Services.AddScoped<IOrderDetailsServiceReader, OrderDetailsReader>();
            Services.AddScoped<IOrderDetailsServiceWriter, OrderDetailsWriter>();
            Services.AddScoped<IOrderDetailsServiceContainer, OrderDetailsContainer>();
            Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            return Services;
        }
    }
}

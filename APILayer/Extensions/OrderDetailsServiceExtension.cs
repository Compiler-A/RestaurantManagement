using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class OrderDetailsServiceExtension
    {
        public static IServiceCollection AddOrderDetailsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IOrderDetailsRepositoryReader, clsOrderDetailsRepositoryReader>();
            Services.AddScoped<IOrderDetailsRepositoryWriter, clsOrderDetailsRepositoryWriter>();
            Services.AddScoped<IOrderDetailsRepository, clsOrderDetailsRepository>();
            Services.AddScoped<IOrderDetailsServiceReader, clsOrderDetailsReader>();
            Services.AddScoped<IOrderDetailsServiceWriter, clsOrderDetailsWriter>();
            Services.AddScoped<IOrderDetailsServiceContainer, clsOrderDetailsContainer>();
            Services.AddScoped<IOrderDetailsServiceComposition, clsOrderLoader>();
            Services.AddScoped<IOrderDetailsServiceComposition, clsMenuItemLoader>();
            Services.AddScoped<IOrderDetailsService, clsOrderDetailsService>();
            return Services;
        }
    }
}

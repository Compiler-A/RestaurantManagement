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
            Services.AddScoped<IReadableDOrderDetails, DataLayerRestaurant.Classes.clsOrderDetailsReader>();
            Services.AddScoped<IWritableDOrderDetails, DataLayerRestaurant.Classes.clsOrderDetailsWriter>();
            Services.AddScoped<IDataOrderDetails, clsDataOrderDetails>();
            Services.AddScoped<IOrderDetailsServiceReader, BusinessLayerRestaurant.Classes.clsOrderDetailsReader>();
            Services.AddScoped<IOrderDetailsServiceWriter, BusinessLayerRestaurant.Classes.clsOrderDetailsWriter>();
            Services.AddScoped<IOrderDetailsServiceContainer, clsOrderDetailsContainer>();
            Services.AddScoped<IOrderDetailsServiceComposition, clsOrderLoader>();
            Services.AddScoped<IOrderDetailsServiceComposition, clsMenuItemLoader>();
            Services.AddScoped<IOrderDetailsService, clsOrderDetailsService>();
            return Services;
        }
    }
}

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
            Services.AddScoped<IReadableBOrderDetails, BusinessLayerRestaurant.Classes.clsOrderDetailsReader>();
            Services.AddScoped<IWritableBOrderDetails, BusinessLayerRestaurant.Classes.clsOrderDetailsWriter>();
            Services.AddScoped<IInterfaceBOrderDetails, clsOrderDetailsRepositoryBridge>();
            Services.AddScoped<ICompositionBOrderDetails, clsOrderLoader>();
            Services.AddScoped<ICompositionBOrderDetails, clsMenuItemLoader>();
            Services.AddScoped<IBusinessOrderDetails, clsBusinessOrderDetails>();
            return Services;
        }
    }
}

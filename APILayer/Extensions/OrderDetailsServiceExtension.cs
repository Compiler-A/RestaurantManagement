using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class OrderDetailsServiceExtension
    {
        public static IServiceCollection AddOrderDetailsServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDOrderDetails, DataLayerRestaurant.clsOrderDetailsReader>();
            Services.AddScoped<IWritableDOrderDetails, DataLayerRestaurant.clsOrderDetailsWriter>();
            Services.AddScoped<IDataOrderDetails, clsDataOrderDetails>();
            Services.AddScoped<IReadableBOrderDetails, BusinessLayerRestaurant.clsOrderDetailsReader>();
            Services.AddScoped<IWritableBOrderDetails, BusinessLayerRestaurant.clsOrderDetailsWriter>();
            Services.AddScoped<IDTOBOrderDetails, clsOrderDetailsDtoContainer>();
            Services.AddScoped<IInterfaceBOrderDetails, clsOrderDetailsRepositoryBridge>();
            Services.AddScoped<ICompositionBOrderDetails, clsOrderLoader>();
            Services.AddScoped<ICompositionBOrderDetails, clsMenuItemLoader>();
            Services.AddScoped<IBusinessOrderDetails, clsBusinessOrderDetails>();
            return Services;
        }
    }
}

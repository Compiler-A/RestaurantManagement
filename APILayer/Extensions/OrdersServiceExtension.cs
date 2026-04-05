using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class OrdersServiceExtension
    {
        public static IServiceCollection AddOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IWritableDOrders, DataLayerRestaurant.clsOrdersWriter>();
            Services.AddScoped<IReadableDOrders, DataLayerRestaurant.clsOrdersReader>();
            Services.AddScoped<IDataOrders, clsDataOrders>();
            Services.AddScoped<IWritableBOrders, BusinessLayerRestaurant.clsOrdersWriter>();
            Services.AddScoped<IReadableBOrders, BusinessLayerRestaurant.clsOrdersReader>();
            Services.AddScoped<IDTOBOrders, clsOrdersDtoContainer>();
            Services.AddScoped<IInterfaceBOrders, clsOrdersRepositoryBridge>();
            Services.AddScoped<ICompositionBOrders, clsStatusOrderLoader>();
            Services.AddScoped<ICompositionBOrders, clsEmployeeLoader>();
            Services.AddScoped<ICompositionBOrders, clsTableLoader>();
            Services.AddScoped<IBusinessOrders, clsBusinessOrders>();
            return Services;
        }
    }
}

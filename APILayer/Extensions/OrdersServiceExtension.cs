using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class OrdersServiceExtension
    {
        public static IServiceCollection AddOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IWritableDOrders, DataLayerRestaurant.Classes.clsOrdersWriter>();
            Services.AddScoped<IReadableDOrders, DataLayerRestaurant.Classes.clsOrdersReader>();
            Services.AddScoped<IDataOrders, clsDataOrders>();
            Services.AddScoped<IWritableBOrders, BusinessLayerRestaurant.Classes.clsOrdersWriter>();
            Services.AddScoped<IReadableBOrders, BusinessLayerRestaurant.Classes.clsOrdersReader>();
            Services.AddScoped<IInterfaceBOrders, clsOrdersRepositoryBridge>();
            Services.AddScoped<ICompositionBOrders, clsStatusOrderLoader>();
            Services.AddScoped<ICompositionBOrders, clsEmployeeLoader>();
            Services.AddScoped<ICompositionBOrders, clsTableLoader>();
            Services.AddScoped<IBusinessOrders, clsBusinessOrders>();
            return Services;
        }
    }
}

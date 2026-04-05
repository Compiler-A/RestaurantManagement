using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class StatusOrdersServiceExtension
    {
        public static IServiceCollection AddStatusOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDStatusOrders, DataLayerRestaurant.clsStatusOrdersReader>();
            Services.AddScoped<IWritableDStatusOrders, DataLayerRestaurant.clsStatusOrdersWriter>();
            Services.AddScoped<IDataStatusOrders, clsDataStatusOrders>();
            Services.AddScoped<IReadableBStatusOrders, BusinessLayerRestaurant.clsStatusOrdersReader>();
            Services.AddScoped<IWritableBStatusOrders, BusinessLayerRestaurant.clsStatusOrdersWriter>();
            Services.AddScoped<IDTOBStatusOrders, clsStatusOrdersDtoContainer>();
            Services.AddScoped<IInterfaceBStatusOrders, clsStatusOrdersRepositoryBridge>();
            Services.AddScoped<IBusinessStatusOrders, clsBusinessStatusOrders>();
            return Services;
        }
    }
}

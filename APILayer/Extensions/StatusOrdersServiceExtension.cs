using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class StatusOrdersServiceExtension
    {
        public static IServiceCollection AddStatusOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDStatusOrders, DataLayerRestaurant.Classes.clsStatusOrdersReader>();
            Services.AddScoped<IWritableDStatusOrders, DataLayerRestaurant.Classes.clsStatusOrdersWriter>();
            Services.AddScoped<IDataStatusOrders, clsDataStatusOrders>();
            Services.AddScoped<IStatusOrdersServiceReader, BusinessLayerRestaurant.Classes.clsStatusOrdersReader>();
            Services.AddScoped<IWritableBStatusOrders, BusinessLayerRestaurant.Classes.clsStatusOrdersWriter>();
            Services.AddScoped<IStatusOrdersServiceContainer, clsStatusOrdersContainer>();
            Services.AddScoped<IStatusOrdersService, clsStatusOrdersService>();
            return Services;
        }
    }
}

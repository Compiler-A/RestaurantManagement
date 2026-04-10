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
            Services.AddScoped<IOrdersServiceWriter, BusinessLayerRestaurant.Classes.clsOrdersWriter>();
            Services.AddScoped<IOrdersServiceReader, BusinessLayerRestaurant.Classes.clsOrdersReader>();
            Services.AddScoped<IOrdersServiceContainer, clsOrdersContainer>();
            Services.AddScoped<IOrdersServiceComposition, clsStatusOrderLoader>();
            Services.AddScoped<IOrdersServiceComposition, clsEmployeeLoader>();
            Services.AddScoped<IOrdersServiceComposition, clsTableLoader>();
            Services.AddScoped<IOrdersService, clsOrdersService>();
            return Services;
        }
    }
}

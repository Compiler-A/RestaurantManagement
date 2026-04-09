using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class StatusTablesServiceExtension
    {
        public static IServiceCollection AddStatusTablesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDStatusTables, DataLayerRestaurant.Classes.clsStatusTablesReader>();
            Services.AddScoped<IWritableDStatusTables, DataLayerRestaurant.Classes.clsStatusTablesWriter>();
            Services.AddScoped<IDataStatusTables, clsDataStatusTables>();
            Services.AddScoped<IReadableBStatusTables, BusinessLayerRestaurant.Classes.clsStatusTablesReader>();
            Services.AddScoped<IWritableBStatusTables, BusinessLayerRestaurant.Classes.clsStatusTablesWriter>();
            Services.AddScoped<IInterfaceBStatusTables, clsStatusTablesRepositoryBridge>();
            Services.AddScoped<IBusinessStatusTables, clsBusinessStatusTables>();

            return Services;
        }
    }
}

using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class StatusTablesServiceExtension
    {
        public static IServiceCollection AddStatusTablesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDStatusTables, DataLayerRestaurant.clsStatusTablesReader>();
            Services.AddScoped<IWritableDStatusTables, DataLayerRestaurant.clsStatusTablesWriter>();
            Services.AddScoped<IDataStatusTables, clsDataStatusTables>();
            Services.AddScoped<IReadableBStatusTables, BusinessLayerRestaurant.clsStatusTablesReader>();
            Services.AddScoped<IWritableBStatusTables, BusinessLayerRestaurant.clsStatusTablesWriter>();
            Services.AddScoped<IDTOBStatusTables, clsStatusTablesDtoContainer>();
            Services.AddScoped<IInterfaceBStatusTables, clsStatusTablesRepositoryBridge>();
            Services.AddScoped<IBusinessStatusTables, clsBusinessStatusTables>();

            return Services;
        }
    }
}

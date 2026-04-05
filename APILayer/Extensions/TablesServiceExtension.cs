using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class TablesServiceExtension
    {
        public static IServiceCollection AddTablesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDTables, DataLayerRestaurant.clsTablesReader>();
            Services.AddScoped<IWritableDTables, DataLayerRestaurant.clsTablesWriter>();
            Services.AddScoped<IDataTables, clsDataTables>();
            Services.AddScoped<IReadableBTables, BusinessLayerRestaurant.clsTablesReader>();
            Services.AddScoped<IWritableBTables, BusinessLayerRestaurant.clsTablesWriter>();
            Services.AddScoped<IDTOBTables, clsTablesDtoContainer>();
            Services.AddScoped<IInterfaceBTables, clsTablesRepositoryBridge>();
            Services.AddScoped<ICompositionBTables, clsStatusTableLoader>();
            Services.AddScoped<IBusinessTables, clsBusinessTables>();
            return Services;
        }
    }
}

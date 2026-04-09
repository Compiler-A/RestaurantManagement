using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class TablesServiceExtension
    {
        public static IServiceCollection AddTablesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDTables, DataLayerRestaurant.Classes.clsTablesReader>();
            Services.AddScoped<IWritableDTables, DataLayerRestaurant.Classes.clsTablesWriter>();
            Services.AddScoped<IDataTables, clsDataTables>();
            Services.AddScoped<IReadableBTables, BusinessLayerRestaurant.Classes.clsTablesReader>();
            Services.AddScoped<IWritableBTables, BusinessLayerRestaurant.Classes.clsTablesWriter>();
            Services.AddScoped<IInterfaceBTables, clsTablesRepositoryBridge>();
            Services.AddScoped<ICompositionBTables, clsStatusTableLoader>();
            Services.AddScoped<IBusinessTables, clsBusinessTables>();
            return Services;
        }
    }
}

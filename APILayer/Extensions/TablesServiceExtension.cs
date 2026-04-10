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
            Services.AddScoped<ITablesServiceReader, BusinessLayerRestaurant.Classes.clsTablesReader>();
            Services.AddScoped<ITablesServiceWriter, BusinessLayerRestaurant.Classes.clsTablesWriter>();
            Services.AddScoped<ITablesServiceContainer, clsTablesContainer>();
            Services.AddScoped<ITablesServiceComposition, clsStatusTableLoader>();
            Services.AddScoped<ITablesService, clsTablesService>();
            return Services;
        }
    }
}

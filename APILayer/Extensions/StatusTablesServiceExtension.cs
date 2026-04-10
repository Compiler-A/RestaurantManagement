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
            Services.AddScoped<IStatusTablesServiceReader, BusinessLayerRestaurant.Classes.clsStatusTablesReader>();
            Services.AddScoped<IStatusTablesServiceWriter, BusinessLayerRestaurant.Classes.clsStatusTablesWriter>();
            Services.AddScoped<IStatusTablesServiceContainer, clsStatusTablesContainer>();
            Services.AddScoped<IStatusTablesService, clsStatusTablesService>();

            return Services;
        }
    }
}

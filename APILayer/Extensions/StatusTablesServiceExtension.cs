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
            Services.AddScoped<IStatusTablesRepositoryReader, clsStatusTablesRepositoryReader>();
            Services.AddScoped<IStatusTablesRepositoryWriter, clsStatusTablesRepositoryWriter>();
            Services.AddScoped<IStatusTablesRepository, clsStatusTablesRepository>();
            Services.AddScoped<IStatusTablesServiceReader, clsStatusTablesReader>();
            Services.AddScoped<IStatusTablesServiceWriter, clsStatusTablesWriter>();
            Services.AddScoped<IStatusTablesServiceContainer, clsStatusTablesContainer>();
            Services.AddScoped<IStatusTablesService, clsStatusTablesService>();

            return Services;
        }
    }
}

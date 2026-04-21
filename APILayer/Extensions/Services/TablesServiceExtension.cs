using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions.Services
{
    public static class TablesServiceExtension
    {
        public static IServiceCollection AddTablesServices(this IServiceCollection Services)
        {
            Services.AddScoped<ITablesRepositoryReader, clsTablesRepositoryReader>();
            Services.AddScoped<ITablesRepositoryWriter, clsTablesRepositoryWriter>();
            Services.AddScoped<ITablesRepository, clsTablesRepository>();
            Services.AddScoped<ITablesServiceReader, clsTablesReader>();
            Services.AddScoped<ITablesServiceWriter, clsTablesWriter>();
            Services.AddScoped<ITablesServiceContainer, clsTablesContainer>();
            Services.AddScoped<ITablesServiceComposition, clsStatusTableLoader>();
            Services.AddScoped<ITablesService, clsTablesService>();
            return Services;
        }
    }
}

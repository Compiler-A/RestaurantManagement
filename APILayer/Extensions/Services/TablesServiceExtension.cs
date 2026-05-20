using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.EF;
using DataLayerRestaurant.Classes.Repository;


namespace APILayer.Extensions.Services
{
    public static class TablesServiceExtension
    {
        public static IServiceCollection AddTablesServices(this IServiceCollection Services)
        {
            Services.AddScoped<ITablesRepositoryReader, TablesRepositoryReader>();
            Services.AddScoped<ITablesRepositoryWriter, TablesRepositoryWriter>();
            Services.AddScoped<ITablesRepository, TablesRepository>();
            Services.AddScoped<ITablesServiceReader, TablesReader>();
            Services.AddScoped<ITablesServiceWriter, TablesWriter>();
            Services.AddScoped<ITablesServiceContainer, TablesContainer>();
            Services.AddScoped<ITablesService, TablesService>();
            return Services;
        }
    }
}

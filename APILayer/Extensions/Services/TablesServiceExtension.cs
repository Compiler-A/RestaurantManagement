using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.EF;
using DataLayerRestaurant.Classes.Repository;


namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddTablesServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<ITablesRepositoryReader, TablesRepositoryReaderEF>();
                Services.AddScoped<ITablesRepositoryWriter, TablesRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<ITablesRepositoryReader, TablesRepositoryReader>();
                Services.AddScoped<ITablesRepositoryWriter, TablesRepositoryWriter>();
            }
            
            Services.AddScoped<ITablesRepository, TablesRepository>();
            Services.AddScoped<ITablesServiceReader, TablesReader>();
            Services.AddScoped<ITablesServiceWriter, TablesWriter>();
            Services.AddScoped<ITablesServiceContainer, TablesContainer>();
            Services.AddScoped<ITablesService, TablesService>();
            return Services;
        }
    }
}

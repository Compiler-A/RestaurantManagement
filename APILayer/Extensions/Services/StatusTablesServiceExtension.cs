using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.EF;
using DataLayerRestaurant.Classes.Repository;

namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddStatusTablesServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IStatusTablesRepositoryReader, StatusTablesRepositoryReaderEF>();
                Services.AddScoped<IStatusTablesRepositoryWriter, StatusTablesRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<IStatusTablesRepositoryReader, StatusTablesRepositoryReader>();
                Services.AddScoped<IStatusTablesRepositoryWriter, StatusTablesRepositoryWriter>();
            }
            
            Services.AddScoped<IStatusTablesRepository, StatusTablesRepository>();
            Services.AddScoped<IStatusTablesServiceReader, StatusTablesReader>();
            Services.AddScoped<IStatusTablesServiceWriter, StatusTablesWriter>();
            Services.AddScoped<IStatusTablesServiceContainer, StatusTablesContainer>();
            Services.AddScoped<IStatusTablesService, StatusTablesService>();

            return Services;
        }
    }
}

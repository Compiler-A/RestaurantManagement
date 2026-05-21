using ContractsLayerRestaurant.Interfaces.Services;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddJobRolesServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IJobRolesRepositoryReader, JobRolesRepositoryReaderEF>();
                Services.AddScoped<IJobRolesRepositoryWriter, JobRolesRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<IJobRolesRepositoryReader, JobRolesRepositoryReader>();
                Services.AddScoped<IJobRolesRepositoryWriter, JobRolesRepositoryWriter>();
            }

            Services.AddScoped<IJobRolesRepository, JobRolesRepository>();
            Services.AddScoped<IJobRolesServiceWriter, JobRolesWriter>();
            Services.AddScoped<IJobRolesServiceReader, JobRolesReader>();
            Services.AddScoped<IJobRolesServiceContainer, JobRolesContainer>();
            Services.AddScoped<IJobRolesService, JobRolesService>();
            return Services;
        }
    }
}

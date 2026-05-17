using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;

namespace APILayer.Extensions.Services
{
    public static class JobRolesServiceExtension
    {
        public static IServiceCollection AddJobRolesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IJobRolesRepositoryReader, JobRolesRepositoryReader>();
            Services.AddScoped<IJobRolesRepositoryWriter, JobRolesRepositoryWriter>();
            Services.AddScoped<IJobRolesRepository, JobRolesRepository>();
            Services.AddScoped<IJobRolesServiceWriter, JobRolesWriter>();
            Services.AddScoped<IJobRolesServiceReader, JobRolesReader>();
            Services.AddScoped<IJobRolesServiceContainer, JobRolesContainer>();
            Services.AddScoped<IJobRolesService, JobRolesService>();
            return Services;
        }
    }
}

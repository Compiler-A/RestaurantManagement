using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class JobRolesServiceExtension
    {
        public static IServiceCollection AddJobRolesServices(this IServiceCollection Services)
        {

            Services.AddScoped<IJobRolesRepositoryReader, clsJobRolesRepositoryReader>();
            Services.AddScoped<IJobRolesRepositoryWriter, clsJobRolesRepositoryWriter>();
            Services.AddScoped<clsJobRolesRepository, clsJobRolesRepository>();
            Services.AddScoped<IJobRolesServiceWriter, clsJobRolesWriter>();
            Services.AddScoped<IJobRolesServiceReader, clsJobRolesReader>();
            Services.AddScoped<IJobRolesServiceContainer, clsJobRolesContainer>();
            Services.AddScoped<IJobRolesService, clsJobRolesService>();
            return Services;
        }
    }
}

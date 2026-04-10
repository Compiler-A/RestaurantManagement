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

            Services.AddScoped<IReadableDJobRoles, DataLayerRestaurant.Classes.clsJobRolesReader>();
            Services.AddScoped<IWritableDJobRoles, DataLayerRestaurant.Classes.clsJobRolesWriter>();
            Services.AddScoped<IDataJobRoles, clsDataJobRoles>();
            Services.AddScoped<IJobRolesServiceWriter, BusinessLayerRestaurant.Classes.clsJobRolesWriter>();
            Services.AddScoped<IJobRolesServiceReader, BusinessLayerRestaurant.Classes.clsJobRolesReader>();
            Services.AddScoped<IJobRolesServiceContainer, clsJobRolesContainer>();
            Services.AddScoped<IJobRolesService, clsJobRolesService>();
            return Services;
        }
    }
}

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
            Services.AddScoped<IWritableBJobRoles, BusinessLayerRestaurant.Classes.clsJobRolesWriter>();
            Services.AddScoped<IReadableBJobRoles, BusinessLayerRestaurant.Classes.clsJobRolesReader>();
            Services.AddScoped<IInterfaceBJobRoles, clsBJobRolesRepositoryBridge>();
            Services.AddScoped<IBusinessJobRoles, clsBusinessJobRoles>();
            return Services;
        }
    }
}

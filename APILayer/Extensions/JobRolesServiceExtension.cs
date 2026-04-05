using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class JobRolesServiceExtension
    {
        public static IServiceCollection AddJobRolesServices(this IServiceCollection Services)
        {

            Services.AddScoped<IReadableDJobRoles, DataLayerRestaurant.clsJobRolesReader>();
            Services.AddScoped<IWritableDJobRoles, DataLayerRestaurant.clsJobRolesWriter>();
            Services.AddScoped<IDataJobRoles, clsDataJobRoles>();
            Services.AddScoped<IWritableBJobRoles, BusinessLayerRestaurant.clsJobRolesWriter>();
            Services.AddScoped<IReadableBJobRoles, BusinessLayerRestaurant.clsJobRolesReader>();
            Services.AddScoped<IInterfaceBJobRoles, clsBJobRolesRepositoryBridge>();
            Services.AddScoped<IDTOBJobRoles, clsJobRolesDtoContainer>();
            Services.AddScoped<IBusinessJobRoles, clsBusinessJobRoles>();
            return Services;
        }
    }
}

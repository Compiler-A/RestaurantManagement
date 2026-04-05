using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class EmployeesServiceExtension
    {
        public static IServiceCollection AddEmployeesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDEmployees, DataLayerRestaurant.clsEmployeesReader>();
            Services.AddScoped<IWritableDEmployees, DataLayerRestaurant.clsEmployeesWriter>();
            Services.AddScoped<IDataEmployees, clsDataEmployees>();
            Services.AddScoped<IReadableBEmployees, BusinessLayerRestaurant.clsEmployeesReader>();
            Services.AddScoped<IWritableBEmployees, BusinessLayerRestaurant.clsEmployeesWriter>();
            Services.AddScoped<IDTOBEmployees, clsEmployeesDtoContainer>();
            Services.AddScoped<IInterfaceBEmployees, clsEmployeesRepositoryBridge>();
            Services.AddScoped<ICompositionBEmployees, clsJobRoleLoader>();
            Services.AddScoped<IBusinessEmployees, clsBusinessEmployees>();
            return Services;
        }
    }
}

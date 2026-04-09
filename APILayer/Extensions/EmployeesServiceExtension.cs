using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class EmployeesServiceExtension
    {
        public static IServiceCollection AddEmployeesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IReadableDEmployees, DataLayerRestaurant.Classes.clsEmployeesReader>();
            Services.AddScoped<IWritableDEmployees, DataLayerRestaurant.Classes.clsEmployeesWriter>();
            Services.AddScoped<IDataEmployees, clsDataEmployees>();
            Services.AddScoped<IReadableBEmployees, BusinessLayerRestaurant.Classes.clsEmployeesReader>();
            Services.AddScoped<IWritableBEmployees, BusinessLayerRestaurant.Classes.clsEmployeesWriter>();
            Services.AddScoped<IInterfaceBEmployees, clsEmployeesRepositoryBridge>();
            Services.AddScoped<ICompositionBEmployees, clsJobRoleLoader>();
            Services.AddScoped<IBusinessEmployees, clsBusinessEmployees>();
            return Services;
        }
    }
}

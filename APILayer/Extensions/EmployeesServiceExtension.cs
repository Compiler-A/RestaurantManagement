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
            Services.AddScoped<IEmployeesServiceReader, BusinessLayerRestaurant.Classes.clsEmployeesReader>();
            Services.AddScoped<IEmployeesServiceWriter, BusinessLayerRestaurant.Classes.clsEmployeesWriter>();
            Services.AddScoped<IEmployeesServiceContainer, clsEmployeesContainer>();
            Services.AddScoped<IEmployeesServiceComposition, clsJobRoleLoader>();
            Services.AddScoped<IEmployeesService, clsEmployeesService>();
            return Services;
        }
    }
}

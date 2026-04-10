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
            Services.AddScoped<IEmployeesRepositoryReader, clsEmployeesRepositoryReader>();
            Services.AddScoped<IEmployeesRepositoryWriter, clsEmployeesRepositoryWriter>();
            Services.AddScoped<IEmployeesRepository, clsEmployeesRepository>();
            Services.AddScoped<IEmployeesServiceReader, clsEmployeesReader>();
            Services.AddScoped<IEmployeesServiceWriter, clsEmployeesWriter>();
            Services.AddScoped<IEmployeesServiceContainer, clsEmployeesContainer>();
            Services.AddScoped<IEmployeesServiceComposition, clsJobRoleLoader>();
            Services.AddScoped<IEmployeesService, clsEmployeesService>();
            return Services;
        }
    }
}

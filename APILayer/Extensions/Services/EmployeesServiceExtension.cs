using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;


namespace APILayer.Extensions.Services
{
    public static class EmployeesServiceExtension
    {
        public static IServiceCollection AddEmployeesServices(this IServiceCollection Services)
        {

            Services.AddScoped<IEmployeesRepositoryReader, EmployeesRepositoryReader>();
            Services.AddScoped<IEmployeesRepositoryWriter, EmployeesRepositoryWriter>();
            Services.AddScoped<IEmployeesRepository, EmployeesRepository>();

            Services.AddScoped<IEmployeesServiceReader, EmployeesReader>();
            Services.AddScoped<IEmployeesServiceWriter, EmployeesWriter>();
            Services.AddScoped<IEmployeesServiceContainer, EmployeesContainer>();
            Services.AddScoped<IEmployeesService, EmployeesService>();
            return Services;
        }
    }
}

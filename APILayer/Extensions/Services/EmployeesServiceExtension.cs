using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;
using DomainLayer.Entities;
using RestaurantDataLayer;

namespace APILayer.Extensions.Services
{
    public static class EmployeesServiceExtension
    {
        public static IServiceCollection AddEmployeesServices(this IServiceCollection Services)
        {
            Services.AddScoped<IRepositoryBatchsLoader<Employee>, clsJobRoleBatchLoader>();
            Services.AddScoped<IEmployeeRepositoryLoader, clsEmployeesRepositoryLoader>();
            Services.AddScoped<IEmployeesRepositoryReader, clsEmployeesRepositoryReader>();
            Services.AddScoped<IEmployeesRepositoryWriter, clsEmployeesRepositoryWriter>();
            Services.AddScoped<IEmployeesRepository, clsEmployeesRepository>();

            Services.AddScoped<IEmployeesServiceReader, clsEmployeesReader>();
            Services.AddScoped<IEmployeesServiceWriter, clsEmployeesWriter>();
            Services.AddScoped<IEmployeesServiceContainer, clsEmployeesContainer>();
            Services.AddScoped<IEmployeesService, clsEmployeesService>();
            return Services;
        }
    }
}

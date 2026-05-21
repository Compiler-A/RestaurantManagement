using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;


namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddEmployeesServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IEmployeesRepositoryReader, EmployeesRepositoryReaderEF>();
                Services.AddScoped<IEmployeesRepositoryWriter, EmployeesRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<IEmployeesRepositoryReader, EmployeesRepositoryReader>();
                Services.AddScoped<IEmployeesRepositoryWriter, EmployeesRepositoryWriter>();
            }
            Services.AddScoped<IEmployeesRepository, EmployeesRepository>();

            Services.AddScoped<IEmployeesServiceReader, EmployeesReader>();
            Services.AddScoped<IEmployeesServiceWriter, EmployeesWriter>();
            Services.AddScoped<IEmployeesServiceContainer, EmployeesContainer>();
            Services.AddScoped<IEmployeesService, EmployeesService>();
            return Services;
        }
    }
}

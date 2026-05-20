using ContractsLayerRestaurant.DTORequest.Employees;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;


namespace ContractsLayerRestaurant.Interfaces.Services
{
    public interface IEmployeesServiceContainer : IServiceContainer<IEmployeesRepository>
    {
        IJobRolesService IBusinessJobRole { get; set; }
    }

    public interface IEmployeesServiceContainers
    {
        IJobRolesService IJobRole { get; set; }
    }



    public interface IEmployeesServiceReader : IServiceReader<Employee>
    {
        Task<Employee?> GetAsync(string UserName);
    }

    public interface IEmployeesServiceWriter 
        : IServiceWriter<Employee, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Changed);

    }
    public interface ICRUDEmployeesService : IEmployeesServiceReader, IEmployeesServiceWriter
    { }

    public interface IEmployeesService : ICRUDEmployeesService, IEmployeesServiceContainers
    {
    }

}

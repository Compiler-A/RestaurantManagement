using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Employees;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Interfaces
{
    public interface IEmployeesServiceContainer : IServiceContainer<IEmployeesRepository>
    {
        IJobRolesService IBusinessJobRole { get; set; }
    }

    public interface IEmployeesServiceContainers
    {
        IJobRolesService IJobRole { get; set; }
    }


    public interface IEmployeesServiceComposition
    {
        Task LoadDataAsync(Employee item);
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

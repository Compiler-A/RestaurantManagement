using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Employees;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Interfaces
{
    public interface IEmployeesServiceContainer : IInterfaceBase<IEmployeesRepository>
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

    public interface IEmployeesServiceReader : IReadableBusinessBase<Employee>
    {
        Task<Employee?> GetAsync(string UserName);
    }

    public interface IEmployeesServiceReadable
    {
        Task<List<Employee>> GetAllEmployeesAsync(int page);
        Task<Employee?> GetEmployeeAsync(int ID);
        Task<Employee?> GetEmployeeAsync(string UserName);
    }

    public interface IEmployeesServiceWriter 
        : IWritableBusinessBase<Employee, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Changed);

    }

    public interface IEmployeesServiceWritable
    {
        Task<Employee?> CreateEmployeeAsync(DTOEmployeesCRequest Request);
        Task<Employee?> UpdateEmployeeAsync(DTOEmployeesURequest Request);
        Task<bool> DeleteEmployeeAsync(int ID);
        Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Changed);
    }

    public interface ICRUDEmployeesService : IEmployeesServiceWritable, IEmployeesServiceReadable
    { }

    public interface IEmployeesService : ICRUDEmployeesService, IEmployeesServiceContainers
    {
    }

}

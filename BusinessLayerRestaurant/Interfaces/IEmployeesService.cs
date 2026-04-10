using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Employees;


namespace BusinessLayerRestaurant.Interfaces
{
    public interface IEmployeesServiceContainer : IInterfaceBase<IDataEmployees>
    {
        IJobRolesService IBusinessJobRole { get; set; }
    }

    public interface IEmployeesServiceContainers
    {
        IJobRolesService IJobRole { get; set; }
    }


    public interface IEmployeesServiceComposition
    {
        Task LoadDataAsync(DTOEmployees item);
    }

    public interface IEmployeesServiceReader : IReadableBusinessBase<DTOEmployees>
    {
        Task<DTOEmployees?> LoginAsync(DTOEmployeesLoginRequest Request);
        Task<DTOEmployees?> GetAsync(string UserName);
    }

    public interface IEmployeesServiceReadable
    {
        Task<DTOEmployees?> GetLoginEmployeeAsync(DTOEmployeesLoginRequest Request);
        Task<List<DTOEmployees>> GetAllEmployeesAsync(int page);
        Task<DTOEmployees?> GetEmployeeAsync(int ID);
        Task<DTOEmployees?> GetEmployeeAsync(string UserName);
    }

    public interface IEmployeesServiceWriter 
        : IWritableBusinessBase<DTOEmployees, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Changed);

    }

    public interface IEmployeesServiceWritable
    {
        Task<DTOEmployees?> CreateEmployeeAsync(DTOEmployeesCRequest Request);
        Task<DTOEmployees?> UpdateEmployeeAsync(DTOEmployeesURequest Request);
        Task<bool> DeleteEmployeeAsync(int ID);
        Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Changed);
    }

    public interface ICRUDEmployeesService : IEmployeesServiceWritable, IEmployeesServiceReadable
    { }

    public interface IEmployeesService : ICRUDEmployeesService, IEmployeesServiceContainers
    {
    }

}

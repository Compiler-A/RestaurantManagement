using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Employees;


namespace BusinessLayerRestaurant.Interfaces
{



    public interface IInterfaceBEmployees : IInterfaceBase<IDataEmployees>
    {
        IBusinessJobRoles IBusinessJobRole { get; set; }
    }

    public interface IInterfaceBusinessEmployee
    {
        IBusinessJobRoles IJobRole { get; set; }
    }


    public interface ICompositionBEmployees
    {
        Task LoadDataAsync(DTOEmployees item);
    }

    public interface IReadableBEmployees : IReadableBusinessBase<DTOEmployees>
    {
        Task<DTOEmployees?> LoginAsync(DTOEmployeesLoginRequest Request);
        Task<DTOEmployees?> GetAsync(string UserName);
    }

    public interface IReadableBusinessEmployees
    {
        Task<DTOEmployees?> GetLoginEmployeeAsync(DTOEmployeesLoginRequest Request);
        Task<List<DTOEmployees>> GetAllEmployeesAsync(int page);
        Task<DTOEmployees?> GetEmployeeAsync(int ID);
        Task<DTOEmployees?> GetEmployeeAsync(string UserName);
    }

    public interface IWritableBEmployees 
        : IWritableBusinessBase<DTOEmployees, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Changed);

    }

    public interface IWritableBusinessEmployees
    {
        Task<DTOEmployees?> CreateEmployeeAsync(DTOEmployeesCRequest Request);
        Task<DTOEmployees?> UpdateEmployeeAsync(DTOEmployeesURequest Request);
        Task<bool> DeleteEmployeeAsync(int ID);
        Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Changed);
    }

    public interface ICRUDBusinessEmployees : IWritableBusinessEmployees, IReadableBusinessEmployees
    { }

    public interface IBusinessEmployees : ICRUDBusinessEmployees, IInterfaceBusinessEmployee
    {
    }

}

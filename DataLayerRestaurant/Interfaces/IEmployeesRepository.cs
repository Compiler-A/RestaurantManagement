using ContractsLayerRestaurant.DTOs.Employees;
using RestaurantDataLayer;


namespace DataLayerRestaurant.Interfaces
{
    public interface IEmployeesRepositoryReader : IReadableDataBase<DTOEmployees>
    {
        Task<DTOEmployees?> GetDataLoginAsync(DTOEmployeesLoginRequest Request);
        Task<DTOEmployees?> GetDataAsync(string UserName);

    }

    public interface IJobRolesRepositoryReadable
    {
        Task<DTOEmployees?> GetLoginEmployeeAsync(DTOEmployeesLoginRequest Request);
        Task<List<DTOEmployees>> GetAllEmployeesAsync(int page);
        Task<DTOEmployees?> GetEmployeeAsync(int ID);
        Task<DTOEmployees?> GetEmployeeAsync(string UserName);
    }

    public interface IEmployeesRepositoryWriter 
        : IWritableDataBase<DTOEmployees, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangedDataPasswordAsync(DTOEmployeesChangedPassword Changed);
    }


    public interface IEmployeesRepositoryWritable
    {
        Task<DTOEmployees?> AddEmployeeAsync(DTOEmployeesCRequest Request);
        Task<DTOEmployees?> UpdateEmployeeAsync(DTOEmployeesURequest Request);
        Task<bool> DeleteEmployeeAsync(int ID);
        Task<bool> ChangedPasswordEmployeeAsync(DTOEmployeesChangedPassword Changed);
    }


    public interface IEmployeesRepository : IJobRolesRepositoryReadable, IEmployeesRepositoryWritable { }

}

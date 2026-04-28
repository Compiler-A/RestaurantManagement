using ContractsLayerRestaurant.DTORequest.Employees;
using RestaurantDataLayer;


namespace DataLayerRestaurant.Interfaces
{
    public interface IEmployeesRepositoryReader : IReadableDataBase<Employee>
    {
        Task<Employee?> GetDataAsync(string UserName);

    }

    public interface IEmployeesRepositoryReadable
    {
        Task<List<Employee>> GetAllEmployeesAsync(int page);
        Task<Employee?> GetEmployeeAsync(int ID);
        Task<Employee?> GetEmployeeAsync(string UserName);
    }

    public interface IEmployeesRepositoryWriter 
        : IWritableDataBase<Employee, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangedDataPasswordAsync(DTOEmployeesChangedPassword Changed);
    }


    public interface IEmployeesRepositoryWritable
    {
        Task<Employee?> AddEmployeeAsync(DTOEmployeesCRequest Request);
        Task<Employee?> UpdateEmployeeAsync(DTOEmployeesURequest Request);
        Task<bool> DeleteEmployeeAsync(int ID);
        Task<bool> ChangedPasswordEmployeeAsync(DTOEmployeesChangedPassword Changed);
    }


    public interface IEmployeesRepository : IEmployeesRepositoryReadable, IEmployeesRepositoryWritable { }

}

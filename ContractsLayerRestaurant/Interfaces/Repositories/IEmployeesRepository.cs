using ContractsLayerRestaurant.DTORequest.Employees;
using DomainLayer.Entities;


namespace ContractsLayerRestaurant.Interfaces.Repositories
{

    public interface IEmployeesRepositoryReader : IRepositoryReader<Employee>
    {
        Task<Employee?> GetDataAsync(string UserName);
    }

    public interface IEmployeesRepositoryWriter 
        : IRepositoryWriter<Employee, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangedDataPasswordAsync(DTOEmployeesChangedPassword Changed);
    }

    public interface IEmployeesRepository : IEmployeesRepositoryReader, IEmployeesRepositoryWriter { }

}

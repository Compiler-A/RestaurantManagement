using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public interface IReadableDEmployees : IReadableDataBase<DTOEmployees>
    {
        Task<DTOEmployees?> GetDataLoginAsync(DTOEmployeesLoginRequest Request);
        Task<DTOEmployees?> GetDataAsync(string UserName);

    }

    public interface IReadableDataEmployees
    {
        Task<DTOEmployees?> GetLoginEmployeeAsync(DTOEmployeesLoginRequest Request);
        Task<List<DTOEmployees>> GetAllEmployeesAsync(int page);
        Task<DTOEmployees?> GetEmployeeAsync(int ID);
        Task<DTOEmployees?> GetEmployeeAsync(string UserName);
    }

    public interface IWritableDEmployees 
        : IWritableDataBase<DTOEmployees, DTOEmployeesCRequest, DTOEmployeesURequest>
    {
        Task<bool> ChangedDataPasswordAsync(DTOEmployeesChangedPassword Changed);
    }


    public interface IWritableDataEmployees
    {
        Task<DTOEmployees?> AddEmployeeAsync(DTOEmployeesCRequest Request);
        Task<DTOEmployees?> UpdateEmployeeAsync(DTOEmployeesURequest Request);
        Task<bool> DeleteEmployeeAsync(int ID);
        Task<bool> ChangedPasswordEmployeeAsync(DTOEmployeesChangedPassword Changed);
    }


    public interface IDataEmployees : IReadableDataEmployees, IWritableDataEmployees { }

}

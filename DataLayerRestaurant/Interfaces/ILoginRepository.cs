using ContractsLayerRestaurant.DTOs.Employees;
using ContractsLayerRestaurant.DTOs.Login;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant.Interfaces
{
    public interface ILoginRepositoryReader
    {
           Task<DTOLogin?> GetDataAsync(string UserName);
    }
    public interface ILoginRepositoryWriter
    {
        Task<bool> UpdateDataAsync(DTOLoginCURequest Request);
        Task<bool> CreateDataAsync(DTOLoginCURequest Request); 
        Task<bool> LogoutDataAsync(int ID);
    }
    public interface ILoginRepository : ILoginRepositoryReader, ILoginRepositoryWriter
    { }

}

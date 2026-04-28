using ContractsLayerRestaurant.DTORequest.Employees;
using ContractsLayerRestaurant.DTORequest.Auth;
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
           Task<Auth?> GetDataAsync(string UserName);
    }
    public interface ILoginRepositoryWriter
    {
        Task<bool> UpdateDataAsync(DTOAuthCURequest Request);
        Task<bool> CreateDataAsync(DTOAuthCURequest Request); 
        Task<bool> LogoutDataAsync(int ID);
    }
    public interface ILoginRepository : ILoginRepositoryReader, ILoginRepositoryWriter
    { }

}

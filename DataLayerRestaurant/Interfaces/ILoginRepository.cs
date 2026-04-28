using ContractsLayerRestaurant.DTORequest.Auth;
using DomainLayer.Entities;


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

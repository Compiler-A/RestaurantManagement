using ContractsLayerRestaurant.DTORequest.Auth;
using DomainLayer.Entities;


namespace ContractsLayerRestaurant.Interfaces.Repositories
{
    public interface ILoginRepositoryReader
    {
        Task<Auth?> LoginAsync(string UserName);
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

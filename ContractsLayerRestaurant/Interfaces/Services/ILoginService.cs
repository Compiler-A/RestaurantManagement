using ContractsLayerRestaurant.DTORequest.Auth;
using DomainLayer.Entities;
using ContractsLayerRestaurant.DTOResponse;
using ContractsLayerRestaurant.Interfaces.Repositories;

namespace ContractsLayerRestaurant.Interfaces.Services
{
    public interface ILoginServiceContainer : IServiceContainer<ILoginRepository>
    {
        IEmployeesService IEmployee { get; set; }
    }


    public interface ILoginServiceReader 
    {
        Task<Auth?> GetAsync(string UserName);
    }


    public interface ILoginServiceWriter
    {
        Task<DTOTokenResponse> LoginAsync(DTOLoginRequest Request);
        Task<DTOTokenResponse> RefrehTokenAsync(DTORefreshRequest Request);
        Task<bool> LogoutAsync(DTOLogoutRequest Request);
    }

    public interface ILoginService : ILoginServiceReader, ILoginServiceWriter
    {
        IEmployeesService IEmployee { get; set; }
    }
}

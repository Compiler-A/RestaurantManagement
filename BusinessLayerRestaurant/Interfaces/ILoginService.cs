using ContractsLayerRestaurant.DTORequest.Auth;
using DataLayerRestaurant.Interfaces;
using RestaurantDataLayer;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface ILoginServiceContainer : IInterfaceBase<ILoginRepository>
    {
        IEmployeesService IEmployee { get; set; }
    }

    public interface ILoginServiceComposition
    {
        Task LoadDataAsync(Auth item);
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

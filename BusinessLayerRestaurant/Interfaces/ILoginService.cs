using ContractsLayerRestaurant.DTOs.Login;
using DataLayerRestaurant.Interfaces;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface ILoginServiceContainer : IInterfaceBase<ILoginRepository>
    {
        IEmployeesService IEmployee { get; set; }
    }

    public interface ILoginServiceComposition
    {
        Task LoadDataAsync(DTOLogin item);
    }

    public interface ILoginServiceReader 
    {
        Task<DTOLogin?> GetAsync(string UserName);
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

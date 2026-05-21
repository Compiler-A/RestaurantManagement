using ContractsLayerRestaurant.DTORequest.Auth;
using ContractsLayerRestaurant.DTOResponse;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Services
{
    public class LoginService : ILoginService
    {
        private ILoginServiceReader _Reader;
        private ILoginServiceWriter _Writer;
        private ILoginServiceContainer _Interface;

        public LoginService(ILoginServiceContainer Interface, ILoginServiceReader Reader, ILoginServiceWriter Writer)
        {
            _Interface = Interface;
            _Reader = Reader;
            _Writer = Writer;
        }

        public IEmployeesService IEmployee
        {
            get => _Interface.IEmployee;
            set => _Interface.IEmployee = value;
        }

        public async Task<Auth?> GetAsync(string UserName)
        {
            return await _Reader.GetAsync(UserName);
        }
        public async Task<DTOTokenResponse> LoginAsync(DTOLoginRequest Request)
        {
            return await _Writer.LoginAsync(Request);
        }
        public async Task<DTOTokenResponse> RefrehTokenAsync(DTORefreshRequest Request)
        {
            return await _Writer.RefrehTokenAsync(Request);
        }

        public async Task<bool> LogoutAsync(DTOLogoutRequest Request)
        {
            return await _Writer.LogoutAsync(Request);
        }

    }
}

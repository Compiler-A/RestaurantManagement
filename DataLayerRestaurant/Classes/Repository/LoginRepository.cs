using ContractsLayerRestaurant.DTORequest.Auth;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Classes.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ILoginRepositoryReader _reader;
        private readonly ILoginRepositoryWriter _writer;
        public LoginRepository(ILoginRepositoryReader reader, ILoginRepositoryWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public async Task<Auth?> LoginAsync(string UserName)
        {
            return await _reader.LoginAsync(UserName);
        }

        public async Task<bool> CreateDataAsync(DTOAuthCURequest Request)
        {
            return await _writer.CreateDataAsync(Request);
        }
        public async Task<bool> LogoutDataAsync(int EmployeeID)
        {
            return await _writer.LogoutDataAsync(EmployeeID);
        }
        public async Task<bool> UpdateDataAsync(DTOAuthCURequest Request)
        {
            return await _writer.UpdateDataAsync(Request);
        }
        public async Task<Auth?> GetDataAsync(string UserName)
        {
            return await _reader.GetDataAsync(UserName);
        }
    }
}

using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using ContractsLayerRestaurant.Interfaces.Services;
using DataLayerRestaurant.Classes.SQL;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.Repository;

namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddLoginServices(this IServiceCollection Services)
        {

            Services.AddScoped<ILoginRepositoryReader, LoginRepositoryReader>();
            Services.AddScoped<ILoginRepositoryWriter, LoginRepositoryWriter>();
            Services.AddScoped<ILoginRepository, LoginRepository>();
            Services.AddScoped<ILoginServiceReader, LoginReader>();
            Services.AddScoped<ILoginServiceWriter, LoginWriter>();
            Services.AddScoped<ILoginServiceContainer, LoginContainer>();
            Services.AddScoped<ILoginService, LoginService>();
            return Services;
        }
    }
}

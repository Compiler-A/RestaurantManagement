using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;
using RestaurantDataLayer;

namespace APILayer.Extensions.Services
{
    public static class LoginServiceExtension
    {
        public static IServiceCollection AddLoginServices(this IServiceCollection Services)
        {
            Services.AddScoped<IAuthsRepositoryLoader, clsAuthsRepositoryLoader>();
            Services.AddScoped<IRepositoryBatchsLoader<Auth>, clsEmployeeBatchLoaderByAuth>();

            Services.AddScoped<ILoginRepositoryReader, clsLoginRepositoryReader>();
            Services.AddScoped<ILoginRepositoryWriter, clsLoginRepositoryWriter>();
            Services.AddScoped<ILoginRepository, clsLoginRepository>();
            Services.AddScoped<ILoginServiceReader, clsLoginReader>();
            Services.AddScoped<ILoginServiceWriter, clsLoginWriter>();
            Services.AddScoped<ILoginServiceContainer, clsLoginContainer>();
            Services.AddScoped<ILoginService, clsLoginService>();
            return Services;
        }
    }
}

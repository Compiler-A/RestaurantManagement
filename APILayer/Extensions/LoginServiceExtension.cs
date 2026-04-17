using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;

namespace APILayer.Extensions
{
    public static class LoginServiceExtension
    {
        public static IServiceCollection AddLoginServices(this IServiceCollection Services)
        {
            Services.AddScoped<ILoginRepositoryReader, clsLoginRepositoryReader>();
            Services.AddScoped<ILoginRepositoryWriter, clsLoginRepositoryWriter>();
            Services.AddScoped<ILoginRepository, clsLoginRepository>();
            Services.AddScoped<ILoginServiceReader, clsLoginReader>();
            Services.AddScoped<ILoginServiceWriter, clsLoginWriter>();
            Services.AddScoped<ILoginServiceContainer, clsLoginContainer>();
            Services.AddScoped<ILoginServiceComposition, clsEmployeeLoaderByLogin>();
            Services.AddScoped<ILoginService, clsLoginService>();
            return Services;
        }
    }
}

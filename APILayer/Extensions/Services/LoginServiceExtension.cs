using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Interfaces;


namespace APILayer.Extensions.Services
{
    public static class LoginServiceExtension
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

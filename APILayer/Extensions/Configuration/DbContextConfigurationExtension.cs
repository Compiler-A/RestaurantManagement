using DataLayerRestaurant;

namespace APILayer.Extensions.Configuration
{
    public static class DbContextConfigurationExtension
    {
        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.Configure<clsMySettings>(
                Configuration.GetSection("MySettings"));
            return Services;
        }
    }
}

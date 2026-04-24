using DataLayerRestaurant;

namespace APILayer.Extensions.Configuration
{
    public static class MySettingConfigurationExtension
    {
        public static IServiceCollection AddMySettingsConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.Configure<clsMySettings>(
                Configuration.GetSection("MySettings"));
            return Services;
        }
    }
}

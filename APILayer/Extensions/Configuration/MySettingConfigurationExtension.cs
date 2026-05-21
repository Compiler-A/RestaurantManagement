using DataLayerRestaurant;
using DataLayerRestaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Extensions.Configuration
{
    public static partial class ConfigurationExtension
    {
        public static IServiceCollection AddMySettingsConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.Configure<clsMySettings>(
               Configuration.GetSection("MySettings"));
            return Services;
        }
    }
}

using DataLayerRestaurant;
using DataLayerRestaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Extensions.Configuration
{
    public static class MySettingConfigurationExtension
    {
        public static IServiceCollection AddMySettingsConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            var setting = Configuration.GetSection("MySettings").Get<clsMySettings>();
            if (setting == null)
            {
                throw new InvalidOperationException(
                    "setting is null");
            }
            Services.AddDbContext<AppDBContext>(option => option.UseSqlServer(setting.ConnectionString));

            return Services;
        }
    }
}

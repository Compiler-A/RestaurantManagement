using DataLayerRestaurant;
using DataLayerRestaurant.Data;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Extensions.Configuration
{
    public static partial class ConfigurationExtension
    {
        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection Services, IConfiguration Configuration)
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

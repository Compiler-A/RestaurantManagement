using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;

namespace APILayer.Extensions
{
    public static class HashingServiceExtension
    {
        public static IServiceCollection AddHashingServices(this IServiceCollection Services)
        {
            Services.AddSingleton<ISHA256, clsSH256>();
            Services.AddSingleton<IHashingService, clsHashingService>();
            Services.AddSingleton<IBCrypt, clsBCrypt>();
            return Services;
        }
    }
}

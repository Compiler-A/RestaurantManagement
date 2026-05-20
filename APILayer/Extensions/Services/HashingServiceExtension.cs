using BusinessLayerRestaurant.Classes;
using ContractsLayerRestaurant.Interfaces.Services;


namespace APILayer.Extensions.Services
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

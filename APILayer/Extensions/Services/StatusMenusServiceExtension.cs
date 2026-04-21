using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions.Services
{
    public static class StatusMenusServiceExtension
    {
        public static IServiceCollection AddStatusMenusServices(this IServiceCollection Services)
        {
            Services.AddScoped<IStatusMenusRepositoryReader, clsStatusMenusRepositoryReader>();
            Services.AddScoped<IStatusMenusRepositoryWriter, clsStatusMenusRepositoryWriter>();
            Services.AddScoped<IStatusMenusRepository, clsStatusMenusRepository>();
            Services.AddScoped<IStatusMenusServiceWriter, clsStatusMenusWriter>();
            Services.AddScoped<IStatusMenusServiceReader, clsStatusMenusReader>();
            Services.AddScoped<IStatusMenusServiceContainer, clsStatusMenusContainer>();
            Services.AddScoped<IStatusMenusService, clsStatusMenusService>();
            return Services;
        }
    }
}

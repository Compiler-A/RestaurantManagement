using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static class StatusMenusServiceExtension
    {
        public static IServiceCollection AddStatusMenusServices(this IServiceCollection Services)
        {
            Services.AddScoped<IStatusMenusRepositoryReader, StatusMenusRepositoryReader>();
            Services.AddScoped<IStatusMenusRepositoryWriter, StatusMenusRepositoryWriter>();
            Services.AddScoped<IStatusMenusRepository, StatusMenusRepository>();
            Services.AddScoped<IStatusMenusServiceWriter, StatusMenusWriter>();
            Services.AddScoped<IStatusMenusServiceReader, StatusMenusReader>();
            Services.AddScoped<IStatusMenusServiceContainer, StatusMenusContainer>();
            Services.AddScoped<IStatusMenusService, StatusMenusService>();
            return Services;
        }
    }
}

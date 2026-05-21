using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddStatusMenusServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IStatusMenusRepositoryReader, StatusMenusRepositoryReaderEF>();
                Services.AddScoped<IStatusMenusRepositoryWriter, StatusMenusRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<IStatusMenusRepositoryReader, StatusMenusRepositoryReader>();
                Services.AddScoped<IStatusMenusRepositoryWriter, StatusMenusRepositoryWriter>();
            }
            
            Services.AddScoped<IStatusMenusRepository, StatusMenusRepository>();
            Services.AddScoped<IStatusMenusServiceWriter, StatusMenusWriter>();
            Services.AddScoped<IStatusMenusServiceReader, StatusMenusReader>();
            Services.AddScoped<IStatusMenusServiceContainer, StatusMenusContainer>();
            Services.AddScoped<IStatusMenusService, StatusMenusService>();
            return Services;
        }
    }
}

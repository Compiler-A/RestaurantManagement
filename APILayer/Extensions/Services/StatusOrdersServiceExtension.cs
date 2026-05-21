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
        public static IServiceCollection AddStatusOrdersServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IStatusOrdersRepositoryReader, StatusOrdersRepositoryReaderEF>();
                Services.AddScoped<IStatusOrdersRepositoryWriter, StatusOrdersRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<IStatusOrdersRepositoryReader, StatusOrdersRepositoryReader>();
                Services.AddScoped<IStatusOrdersRepositoryWriter, StatusOrdersRepositoryWriter>();
            }
            
            Services.AddScoped<IStatusOrdersRepository, StatusOrdersRepository>();
            Services.AddScoped<IStatusOrdersServiceReader, StatusOrdersReader>();
            Services.AddScoped<IStatusOrdersServiceWriter, StatusOrdersWriter>();
            Services.AddScoped<IStatusOrdersServiceContainer, StatusOrdersContainer>();
            Services.AddScoped<IStatusOrdersService, StatusOrdersService>();
            return Services;
        }
    }
}

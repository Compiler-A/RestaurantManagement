using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.Interfaces.Repositories;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static class StatusOrdersServiceExtension
    {
        public static IServiceCollection AddStatusOrdersServices(this IServiceCollection Services)
        {
            Services.AddScoped<IStatusOrdersRepositoryReader, StatusOrdersRepositoryReaderEF>();
            Services.AddScoped<IStatusOrdersRepositoryWriter, StatusOrdersRepositoryWriterEF>();
            Services.AddScoped<IStatusOrdersRepository, StatusOrdersRepository>();
            Services.AddScoped<IStatusOrdersServiceReader, StatusOrdersReader>();
            Services.AddScoped<IStatusOrdersServiceWriter, StatusOrdersWriter>();
            Services.AddScoped<IStatusOrdersServiceContainer, StatusOrdersContainer>();
            Services.AddScoped<IStatusOrdersService, StatusOrdersService>();
            return Services;
        }
    }
}

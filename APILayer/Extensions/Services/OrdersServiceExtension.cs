using ContractsLayerRestaurant.Interfaces.Services;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.Repository;
using DataLayerRestaurant.Classes.EF;

namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddOrdersServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IOrderDetailBatchLoader, DataLayerRestaurant.Classes.EF.OrderDetailBatchLoader>();
                Services.AddScoped<IOrdersRepositoryWriter, OrdersRepositoryWriterEF>();
                Services.AddScoped<IOrdersRepositoryReader, OrdersRepositoryReaderEF>();
            }
            else
            {
                Services.AddScoped<IOrderDetailBatchLoader, DataLayerRestaurant.Classes.SQL.OrderDetailBatchLoader>();
                Services.AddScoped<IOrdersRepositoryWriter, OrdersRepositoryWriter>();
                Services.AddScoped<IOrdersRepositoryReader, OrdersRepositoryReader>();
            }


            Services.AddScoped<IOrdersRepository, OrdersRepository>();
            Services.AddScoped<IOrdersServiceWriter, OrdersWriter>();
            Services.AddScoped<IOrdersServiceReader, OrdersReader>();
            Services.AddScoped<IOrdersServiceContainer, OrdersContainer>();
            Services.AddScoped<IOrdersService, OrdersService>();
            return Services;
        }
    }
}

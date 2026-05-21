using ContractsLayerRestaurant.Interfaces.Services;
using BusinessLayerRestaurant.Services;
using BusinessLayerRestaurant.Operations;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Classes.SQL;
using DataLayerRestaurant.Classes.EF;
using DataLayerRestaurant.Classes.Repository;


namespace APILayer.Extensions.Services
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddOrderDetailsServices(this IServiceCollection Services, string DataAccessStrategy)
        {

            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<IOrderDetailsRepositoryReader, OrderDetailsRepositoryReaderEF>();
                Services.AddScoped<IOrderDetailsRepositoryWriter, OrderDetailsRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<IOrderDetailsRepositoryReader, OrderDetailsRepositoryReader>();
                Services.AddScoped<IOrderDetailsRepositoryWriter, OrderDetailsRepositoryWriter>();
            }
            Services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            Services.AddScoped<IOrderDetailsServiceReader, OrderDetailsReader>();
            Services.AddScoped<IOrderDetailsServiceWriter, OrderDetailsWriter>();
            Services.AddScoped<IOrderDetailsServiceContainer, OrderDetailsContainer>();
            Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            return Services;
        }
    }
}

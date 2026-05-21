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
        public static IServiceCollection AddTypeItemsServices(this IServiceCollection Services, string DataAccessStrategy)
        {
            if (DataAccessStrategy == "EF")
            {
                Services.AddScoped<ITypeItemsRepositoryReader, TypeItemsRepositoryReaderEF>();
                Services.AddScoped<ITypeItemsRepositoryWriter, TypeItemsRepositoryWriterEF>();
            }
            else
            {
                Services.AddScoped<ITypeItemsRepositoryReader, TypeItemsRepositoryReader>();
                Services.AddScoped<ITypeItemsRepositoryWriter, TypeItemsRepositoryWriter>();
            }
            
            Services.AddScoped<ITypeItemsRepository, TypeItemsRepository>();
            Services.AddScoped<ITypeItemsServiceContainer, TypeItemsContainer>();
            Services.AddScoped<ITypeItemsServiceReader, TypeItemsReader>();
            Services.AddScoped<ITypeItemsServiceWriter, TypeItemsWriter>();
            Services.AddScoped<ITypeItemsService, TypeItemsService>();

            return Services;
        }
    }
}

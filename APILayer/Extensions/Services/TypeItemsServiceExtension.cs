using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes.SQL;

namespace APILayer.Extensions.Services
{
    public static class TypeItemsServiceExtension
    {
        public static IServiceCollection AddTypeItemsServices(this IServiceCollection Services)
        {

            Services.AddScoped<ITypeItemsRepositoryReader, TypeItemsRepositoryReader>();
            Services.AddScoped<ITypeItemsRepositoryWriter, TypeItemsRepositoryWriter>();
            Services.AddScoped<ITypeItemsRepository, TypeItemsRepository>();
            Services.AddScoped<ITypeItemsServiceContainer, TypeItemsContainer>();
            Services.AddScoped<ITypeItemsServiceReader, TypeItemsReader>();
            Services.AddScoped<ITypeItemsServiceWriter, TypeItemsWriter>();
            Services.AddScoped<ITypeItemsService, TypeItemsService>();

            return Services;
        }
    }
}

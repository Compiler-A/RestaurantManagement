using BusinessLayerRestaurant.Interfaces;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Classes;

namespace APILayer.Extensions
{
    public static class TypeItemsServiceExtension
    {
        public static IServiceCollection AddTypeItemsServices(this IServiceCollection Services)
        {

            Services.AddScoped<ITypeItemsRepositoryReader, clsTypeItemsRepositoryReader>();
            Services.AddScoped<ITypeItemsRepositoryWriter, clsTypeItemsRepositoryWriter>();
            Services.AddScoped<ITypeItemsRepository, clsTypeItemsRepository>();
            Services.AddScoped<ITypeItemsServiceContainer, clsTypeItemsContainer>();
            Services.AddScoped<ITypeItemsServiceReader, clsTypeItemsReader>();
            Services.AddScoped<ITypeItemsServiceWriter, clsTypeItemsWriter>();
            Services.AddScoped<ITypeItemsService, clsTypeItemsService>();

            return Services;
        }
    }
}

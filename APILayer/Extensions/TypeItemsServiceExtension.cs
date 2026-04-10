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

            Services.AddScoped<IReadableDTypeItems, DataLayerRestaurant.Classes.clsTypeItemsReader>();
            Services.AddScoped<IWritableDTypeItems, DataLayerRestaurant.Classes.clsTypeItemsWriter>();
            Services.AddScoped<IDataTypeItems, clsDataTypeItems>();
            Services.AddScoped<ITypeItemsServiceContainer, clsTypeItemsContainer>();
            Services.AddScoped<ITypeItemsServiceReader, BusinessLayerRestaurant.Classes.clsTypeItemsReader>();
            Services.AddScoped<ITypeItemsServiceWriter, BusinessLayerRestaurant.Classes.clsTypeItemsWriter>();
            Services.AddScoped<IBusinessService, clsTypeItemsService>();

            return Services;
        }
    }
}

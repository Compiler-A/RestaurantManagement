using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Extensions
{
    public static class TypeItemsServiceExtension
    {
        public static IServiceCollection AddTypeItemsServices(this IServiceCollection Services)
        {

            Services.AddScoped<IReadableDTypeItems, DataLayerRestaurant.clsTypeItemsReader>();
            Services.AddScoped<IWritableDTypeItems, DataLayerRestaurant.clsTypeItemsWriter>();
            Services.AddScoped<IDataTypeItems, clsDataTypeItems>();
            Services.AddScoped<IDTOBTypeItems, clsTypeItemsDtoContainer>();
            Services.AddScoped<IInterfaceBTypeItems, clsTypeItemsRepositoryBridge>();
            Services.AddScoped<IReadableBTypeItems, BusinessLayerRestaurant.clsTypeItemsReader>();
            Services.AddScoped<IWritableBTypeItems, BusinessLayerRestaurant.clsTypeItemsWriter>();
            Services.AddScoped<IBusinessTypeItems, clsBusinessTypeItems>();

            return Services;
        }
    }
}

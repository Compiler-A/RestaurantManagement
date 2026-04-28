using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.TypeItems;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Interfaces
{

    public interface ITypeItemsRepositoryReader : IRepositoryReader<TypeItem>
    { }

    public interface ITypeItemsRepositoryWriter 
        : IRepositoryWriter<TypeItem, DTOTypeItemsCRequest, DTOTypeItemsURequest>
    { }


    public interface ITypeItemsRepository : ITypeItemsRepositoryReader, ITypeItemsRepositoryWriter
    {
    }
}

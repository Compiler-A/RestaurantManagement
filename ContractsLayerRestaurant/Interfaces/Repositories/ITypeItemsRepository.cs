using ContractsLayerRestaurant.DTORequest.TypeItems;
using DomainLayer.Entities;


namespace ContractsLayerRestaurant.Interfaces.Repositories
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

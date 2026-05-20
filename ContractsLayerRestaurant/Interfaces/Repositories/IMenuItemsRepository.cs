using ContractsLayerRestaurant.DTORequest.MenuItems;
using DomainLayer.Entities;

namespace ContractsLayerRestaurant.Interfaces.Repositories
{
    public interface IMenuItemsRepositoryReader : IRepositoryReader<MenuItem>
    {
        Task<List<MenuItem>> GetAllDataAvailablesAsync();
        Task<List<MenuItem>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request);
    }

    public interface IMenuItemsRepositoryWriter : IRepositoryWriter<MenuItem, DTOMenuItemsCRequest, DTOMenuItemsURequest>
    { }


    public interface IMenuItemsRepository : IMenuItemsRepositoryReader, IMenuItemsRepositoryWriter
    {
    }
}

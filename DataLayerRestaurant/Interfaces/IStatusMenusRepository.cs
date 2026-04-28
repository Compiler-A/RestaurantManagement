using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.StatusMenus;

namespace DataLayerRestaurant.Interfaces
{

    public interface IStatusMenusRepositoryReader : IReadableDataBase<StatusMenu>
    {

    }

    public interface IStatusMenusRepositoryWriter 
        : IWritableDataBase<StatusMenu, DTOStatusMenusCRequest, DTOStatusMenusURequest>
    {

    }

    public interface IStatusMenusRepositoryReadable
    {
        Task<StatusMenu?> GetStatusMenuAsync(int ID);
        Task<List<StatusMenu>> GetAllStatusMenusAsync(int Page);
    }

    public interface IStatusMenusRepositoryWritable
    {
        Task<StatusMenu?> AddStatusMenuAsync(DTOStatusMenusCRequest Request);
        Task<StatusMenu?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request);
        Task<bool> DeleteStatusMenuAsync(int id);
    }



    public interface IStatusMenusRepository : IStatusMenusRepositoryReadable, IStatusMenusRepositoryWritable
    {
        
    }
}

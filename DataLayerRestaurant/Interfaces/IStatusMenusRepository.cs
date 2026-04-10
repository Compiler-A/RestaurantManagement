using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.StatusMenus;

namespace DataLayerRestaurant.Interfaces
{

    public interface IStatusMenusRepositoryReader : IReadableDataBase<DTOStatusMenus>
    {

    }

    public interface IStatusMenusRepositoryWriter 
        : IWritableDataBase<DTOStatusMenus, DTOStatusMenusCRequest, DTOStatusMenusURequest>
    {

    }

    public interface IStatusMenusRepositoryReadable
    {
        Task<DTOStatusMenus?> GetStatusMenuAsync(int ID);
        Task<List<DTOStatusMenus>> GetAllStatusMenusAsync(int Page);
    }

    public interface IStatusMenusRepositoryWritable
    {
        Task<DTOStatusMenus?> AddStatusMenuAsync(DTOStatusMenusCRequest Request);
        Task<DTOStatusMenus?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request);
        Task<bool> DeleteStatusMenuAsync(int id);
    }



    public interface IStatusMenusRepository : IStatusMenusRepositoryReadable, IStatusMenusRepositoryWritable
    {
        
    }
}

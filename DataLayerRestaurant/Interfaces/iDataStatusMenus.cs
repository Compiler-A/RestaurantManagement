using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public interface IReadableDStatusMenus : IReadableDataBase<DTOStatusMenus>
    {

    }

    public interface IWritableDStatusMenus 
        : IWritableDataBase<DTOStatusMenus, DTOStatusMenusCRequest, DTOStatusMenusURequest>
    {

    }

    public interface IReadableDataStatusMenus
    {
        Task<DTOStatusMenus?> GetStatusMenuAsync(int ID);
        Task<List<DTOStatusMenus>> GetAllStatusMenusAsync(int Page);
    }

    public interface IWritableDataStatusMenus
    {
        Task<DTOStatusMenus?> AddStatusMenuAsync(DTOStatusMenusCRequest Request);
        Task<DTOStatusMenus?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request);
        Task<bool> DeleteStatusMenuAsync(int id);
    }



    public interface IDataStatusMenus : IReadableDataStatusMenus, IWritableDataStatusMenus
    {
        
    }
}

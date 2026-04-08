using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public interface IInterfaceBStatusMenus : IInterfaceBase<IDataStatusMenus>
    { }

    public interface IReadableBStatusMenus : IReadableBusinessBase<DTOStatusMenus>
    { }

    public interface IWritableBStatusMenus 
        : IWritableBusinessBase<DTOStatusMenus, DTOStatusMenusCRequest, DTOStatusMenusURequest>
    { }

    public interface IReadableBusinessStatusMenus
    {
        Task<List<DTOStatusMenus>> GetAllStatusMenusAsync(int page);
        Task<DTOStatusMenus?> GetStatusMenuAsync(int id);
    }

    public interface IWritableBusinessStatusMenus
    {
        Task<DTOStatusMenus?> AddStatusMenuAsync(DTOStatusMenusCRequest Request);
        Task<DTOStatusMenus?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request);
        Task<bool> DeleteStatusMenuAsync(int ID);
    }
    public interface ICRUDBusinessStatusMenus : IReadableBusinessStatusMenus , IWritableBusinessStatusMenus
    { }

    public interface IBusinessStatusMenus : ICRUDBusinessStatusMenus
    { }

}

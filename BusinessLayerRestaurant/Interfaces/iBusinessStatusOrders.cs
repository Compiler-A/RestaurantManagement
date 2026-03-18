using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IReadableBStatusOrders : IReadableBusinessBase<DTOStatusOrders>
    { }
    public interface IWritableBStatusOrders :
        IWritableBusinessBase<DTOStatusOrders, DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }

    public interface IDTOBStatusOrders : IDTOBase<DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }


    public interface IInterfaceBStatusOrders : IInterfaceBase<IDataStatusOrders>
    { }

    public interface IReadableBusinessStatusOrders
    {
        Task<List<DTOStatusOrders>> GetAllStatusOrdersAsync(int Page);
        Task<DTOStatusOrders?> GetStatusOrdersAsync(int ID);
    }

    public interface IWritableBusinessStatusOrders
    {
        Task<DTOStatusOrders?> AddStatusOrdersAsync(DTOStatusOrdersCRequest Request);
        Task<DTOStatusOrders?> UpdateStatusOrdersAsync(DTOStatusOrdersURequest Request);
        Task<bool> DeleteStatusOrdersAsync(int ID);
    }
    public interface ICRUDBusinessStatusOrders : IWritableBusinessStatusOrders, IReadableBusinessStatusOrders
    { }

    public interface IBusinessStatusOrders : ICRUDBusinessStatusOrders, IDTOBStatusOrders
    { }
}

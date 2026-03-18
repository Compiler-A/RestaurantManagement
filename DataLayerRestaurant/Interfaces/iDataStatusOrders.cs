using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public interface IReadableDStatusOrders : IReadableDataBase<DTOStatusOrders>
    { }
    public interface IWritableDStatusOrders : IWritableDataBase<DTOStatusOrders,DTOStatusOrdersCRequest, DTOStatusOrdersURequest>
    { }

    public interface IReadableDataStatusOrders
    {
        Task<DTOStatusOrders?> GetStatusOrderAsync(int ID);
        Task<List<DTOStatusOrders>> GetAllStatusOrdersAsync(int Page);
    }
    public interface IWritableStatusOrdersData
    {
        Task<DTOStatusOrders?> AddStatusOrderAsync(DTOStatusOrdersCRequest Request);
        Task<DTOStatusOrders?> UpdateStatusOrderAsync(DTOStatusOrdersURequest Request);
        Task<bool> DeleteStatusOrderAsync(int ID);
    }
    public interface IDataStatusOrders : IReadableDataStatusOrders, IWritableStatusOrdersData { }

}

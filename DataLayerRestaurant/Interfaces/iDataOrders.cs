using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public interface IReadableDOrders : IReadableDataBase<DTOOrders>
    {
        Task<List<DTOOrders>?> GetFilterDataAsync(int Page, int TableID, int EmployeeID, int StatusOrderID);

    }
    public interface IWritableDOrders : IWritableDataBase<DTOOrders,DTOOrderCRequest, DTOOrderURequest>
    { }
    public interface IReadableDataOrders
    {
        Task<List<DTOOrders>> GetAllOrdersAsync(int page);
        Task<DTOOrders?> GetOrderAsync(int ID);
        Task<List<DTOOrders>?> GetFilterOrderAsync(int Page, int TableID, int EmployeeID, int StatusOrderID);
    }
    public interface IWritableDataOrders
    {
        Task<DTOOrders?> AddOrderAsync(DTOOrderCRequest order);
        Task<DTOOrders?> UpdateOrderAsync(DTOOrderURequest order);
        Task<bool> DeleteOrderAsync(int id);
    }
    public interface IDataOrders : IReadableDataOrders, IWritableDataOrders
    { }
}

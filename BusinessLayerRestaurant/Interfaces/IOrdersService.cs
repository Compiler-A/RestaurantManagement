using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Orders;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IOrdersServiceReader : IReadableBusinessBase<DTOOrders>
    {
        Task<List<DTOOrders>?> GetFilterAsync(DTOOrderFilterRequest Request);
    }
    public interface IOrdersServiceWriter : IWritableBusinessBase<DTOOrders, DTOOrderCRequest, DTOOrderURequest>
    { }

    public interface IOrdersServiceContainer : IInterfaceBase<IOrdersRepository>
    {
        IStatusOrdersService IBusinessStatusOrder { get; set; }
        IEmployeesService IBusinessEmployee { get; set; }
        ITablesService IBusinessTable { get; set; }
    }

    public interface IOrdersServiceComposition
    {
        Task LoadDataAsync(DTOOrders item);
    }


    public interface IOrdersServiceContainers
    {
        IStatusOrdersService IStatusOrder { get; set; }
        IEmployeesService IEmployee { get; set; }
        ITablesService ITable { get; set; }
    }
    public interface IOrdersServiceReadable
    {
        Task<List<DTOOrders>> GetAllOrdersAsync(int page);
        Task<DTOOrders?> GetOrderAsync(int ID);
        Task<List<DTOOrders>?> GetFilterOrdersAsync(DTOOrderFilterRequest Request);
    }
    public interface IOrdersServiceWritable
    {
        Task<DTOOrders?> AddOrderAsync(DTOOrderCRequest Request);
        Task<DTOOrders?> UpdateOrderAsync(DTOOrderURequest Request);
        Task<bool> DeleteOrderAsync(int ID);
    }

    public interface ICRUDOrdersService : IOrdersServiceWritable, IOrdersServiceReadable
    { }
    public interface IOrdersService : ICRUDOrdersService, IOrdersServiceContainers
    { }

}

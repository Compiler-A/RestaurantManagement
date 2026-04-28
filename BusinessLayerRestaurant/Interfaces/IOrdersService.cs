using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Orders;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IOrdersServiceReader : IServiceReader<Order>
    {
        Task<List<Order>?> GetFilterAsync(DTOOrderFilterRequest Request);
    }
    public interface IOrdersServiceWriter : IServiceWriter<Order, DTOOrderCRequest, DTOOrderURequest>
    { }

    public interface IOrdersServiceContainer : IServiceContainer<IOrdersRepository>
    {
        IStatusOrdersService IBusinessStatusOrder { get; set; }
        IEmployeesService IBusinessEmployee { get; set; }
        ITablesService IBusinessTable { get; set; }
    }

    public interface IOrdersServiceComposition
    {
        Task LoadDataAsync(Order item);
    }


    public interface IOrdersServiceContainers
    {
        IStatusOrdersService IStatusOrder { get; set; }
        IEmployeesService IEmployee { get; set; }
        ITablesService ITable { get; set; }
    }
    public interface IOrdersServiceReadable
    {
        Task<List<Order>> GetAllOrdersAsync(int page);
        Task<Order?> GetOrderAsync(int ID);
        Task<List<Order>?> GetFilterOrdersAsync(DTOOrderFilterRequest Request);
    }
    public interface IOrdersServiceWritable
    {
        Task<Order?> AddOrderAsync(DTOOrderCRequest Request);
        Task<Order?> UpdateOrderAsync(DTOOrderURequest Request);
        Task<bool> DeleteOrderAsync(int ID);
    }

    public interface ICRUDOrdersService : IOrdersServiceWritable, IOrdersServiceReadable
    { }
    public interface IOrdersService : ICRUDOrdersService, IOrdersServiceContainers
    { }

}

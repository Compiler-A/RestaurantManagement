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


    public interface ICRUDOrdersService : IOrdersServiceWriter, IOrdersServiceReader
    { }
    public interface IOrdersService : ICRUDOrdersService, IOrdersServiceContainers
    { }

}

using ContractsLayerRestaurant.DTORequest.Orders;
using DomainLayer.Entities;
using ContractsLayerRestaurant.Interfaces.Repositories;

namespace ContractsLayerRestaurant.Interfaces.Services
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

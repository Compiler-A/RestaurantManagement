using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Tables;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface ITablesServiceContainer : IServiceContainer<ITablesRepository>
    {
        IStatusTablesService IBusinessStatusTable { get; set; }
    }

    public interface ITablesServiceComposition
    {
        Task LoadDataAsync(Table item);
    }

    public interface ITablesServiceContainers
    {
        IStatusTablesService IStatusTable { get; set; }
    }

    public interface ITablesServiceReader : IServiceReader<Table>
    {
        Task<List<Table>> GetAllAsync();
        Task<List<Table>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request);
        Task<List<Table>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request);
        Task<Table?> GetByNameAsync(string tableNumber);
        Task<List<Table>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<List<Table>> GetAllAvailablesAsync();
    }

    public interface ITablesServiceWriter
       : IServiceWriter<Table, DTOTablesCRequest, DTOTablesURequest>
    {
    }

    public interface ICRUDTablesService : ITablesServiceReader, ITablesServiceWriter
    { }

    public interface ITablesService : ICRUDTablesService, ITablesServiceContainers
    {
    }
}

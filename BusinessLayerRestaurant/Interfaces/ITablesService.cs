using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Tables;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface ITablesServiceContainer : IInterfaceBase<ITablesRepository>
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

    public interface ITablesServiceReader : IReadableBusinessBase<Table>
    {
        Task<List<Table>> GetAllAsync();
        Task<List<Table>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request);
        Task<List<Table>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request);
        Task<Table?> GetByNameAsync(string tableNumber);
        Task<List<Table>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<List<Table>> GetAllAvailablesAsync();
    }

    public interface ITablesServiceWriter
       : IWritableBusinessBase<Table, DTOTablesCRequest, DTOTablesURequest>
    {
    }

    public interface ITablesServiceReadable
    {
        Task<List<Table>> GetAllTablesAsync(int page);
        Task<Table?> GetTableAsync(int id);
        Task<List<Table>> GetAllTablesAsync();
        Task<List<Table>> GetTablesFilter1Async(DTOTablesFilterStatusTableRequest Request);
        Task<List<Table>> GetTablesFilter2Async(DTOTablesFilterSeatTableRequest Request);
        Task<Table?> GetTableByNameAsync(string tableNumber);
        Task<List<Table>> GetTablesFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<List<Table>> GetAllTablesAvailablesAsync();
    }
    public interface ITablesServiceWritable
    {
        Task<Table?> AddTableAsync(DTOTablesCRequest Request);
        Task<Table?> UpdateTableAsync(DTOTablesURequest Request);
        Task<bool> DeleteTableAsync(int ID);
    }
    public interface ICRUDTablesService : ITablesServiceReadable, ITablesServiceWritable
    { }

    public interface ITablesService : ICRUDTablesService, ITablesServiceContainers
    {
    }
}

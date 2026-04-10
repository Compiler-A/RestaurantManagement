using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Tables;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface ITablesServiceContainer : IInterfaceBase<IDataTables>
    {
        IStatusTablesService IBusinessStatusTable { get; set; }
    }

    public interface ITablesServiceComposition
    {
        Task LoadDataAsync(DTOTables item);
    }

    public interface ITablesServiceContainers
    {
        IStatusTablesService IStatusTable { get; set; }
    }

    public interface ITablesServiceReader : IReadableBusinessBase<DTOTables>
    {
        Task<List<DTOTables>> GetAllAsync();
        Task<List<DTOTables>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request);
        Task<List<DTOTables>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request);
        Task<DTOTables?> GetByNameAsync(string tableNumber);
        Task<List<DTOTables>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<List<DTOTables>> GetAllAvailablesAsync();
    }

    public interface ITablesServiceWriter
       : IWritableBusinessBase<DTOTables, DTOTablesCRequest, DTOTablesURequest>
    {
    }

    public interface ITablesServiceReadable
    {
        Task<List<DTOTables>> GetAllTablesAsync(int page);
        Task<DTOTables?> GetTableAsync(int id);
        Task<List<DTOTables>> GetAllTablesAsync();
        Task<List<DTOTables>> GetTablesFilter1Async(DTOTablesFilterStatusTableRequest Request);
        Task<List<DTOTables>> GetTablesFilter2Async(DTOTablesFilterSeatTableRequest Request);
        Task<DTOTables?> GetTableByNameAsync(string tableNumber);
        Task<List<DTOTables>> GetTablesFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<List<DTOTables>> GetAllTablesAvailablesAsync();
    }
    public interface ITablesServiceWritable
    {
        Task<DTOTables?> AddTableAsync(DTOTablesCRequest Request);
        Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Request);
        Task<bool> DeleteTableAsync(int ID);
    }
    public interface ICRUDTablesService : ITablesServiceReadable, ITablesServiceWritable
    { }

    public interface ITablesService : ICRUDTablesService, ITablesServiceContainers
    {
    }
}

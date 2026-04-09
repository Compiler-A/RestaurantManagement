using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Tables;

namespace BusinessLayerRestaurant.Interfaces
{

    public interface IInterfaceBTables : IInterfaceBase<IDataTables>
    {
        IBusinessStatusTables IBusinessStatusTable{ get; set; }
    }

    public interface ICompositionBTables
    {
        Task LoadDataAsync(DTOTables item);
    }

    public interface IInterfaceBusinessTables
    {
        IBusinessStatusTables IStatusTable { get; set; }
    }

    public interface IReadableBTables : IReadableBusinessBase<DTOTables>
    {
        Task<List<DTOTables>> GetAllAsync();
        Task<List<DTOTables>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request);
        Task<List<DTOTables>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request);
        Task<DTOTables?> GetByNameAsync(string tableNumber);
        Task<List<DTOTables>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<List<DTOTables>> GetAllAvailablesAsync();
    }

    public interface IWritableBTables
       : IWritableBusinessBase<DTOTables, DTOTablesCRequest, DTOTablesURequest>
    {
    }

    public interface IReadableBusinessTables
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
    public interface IWritableBusinessTables
    {
        Task<DTOTables?> AddTableAsync(DTOTablesCRequest Request);
        Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Request);
        Task<bool> DeleteTableAsync(int ID);
    }
    public interface ICRUDBusinessTables : IReadableBusinessTables, IWritableBusinessTables
    { }

    public interface IBusinessTables : ICRUDBusinessTables, IInterfaceBusinessTables
    {
    }
}

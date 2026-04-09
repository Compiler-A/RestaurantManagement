using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.Tables;


namespace DataLayerRestaurant.Interfaces
{
    public interface IReadableDTables : IReadableDataBase<DTOTables>
    {
        Task<List<DTOTables>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request);
        Task<List<DTOTables>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request);
        Task<List<DTOTables>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<DTOTables?> GetDataByNameAsync(string TableNumber);
        Task<List<DTOTables>> GetAllDataAsync();
        Task<List<DTOTables>> GetAllDataAvailablesAsync();
    }

    public interface IWritableDTables : IWritableDataBase<DTOTables,DTOTablesCRequest, DTOTablesURequest>
    { 
    }


    public interface IReadableDataTables
    {
        Task<DTOTables?> GetTableAsync(int id);
        Task<List<DTOTables>> GetAlltablesAsync(int page);
        Task<List<DTOTables>> GetFilterStatusTablesAsync(DTOTablesFilterStatusTableRequest Request);
        Task<List<DTOTables>> GetFilterSeatTablesAsync(DTOTablesFilterSeatTableRequest Request);
        Task<List<DTOTables>> GetFilterStatusAndSeatTablesAsync(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<DTOTables?> GetTableByNameAsync(string TableNumber);
        Task<List<DTOTables>> GetAlltablesAsync();
        Task<List<DTOTables>> GetAllTablesAvailablesAsync();
    }

    public interface IWritableDataTables
    {
        Task<DTOTables?> AddTableAsync(DTOTablesCRequest Table);
        Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Table);
        Task<bool> DeleteTableAsync(int id);
    }



    public interface IDataTables : IReadableDataTables, IWritableDataTables
    { }
}

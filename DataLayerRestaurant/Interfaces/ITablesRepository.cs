using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.Tables;


namespace DataLayerRestaurant.Interfaces
{
    public interface ITablesRepositoryReader : IReadableDataBase<DTOTables>
    {
        Task<List<DTOTables>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request);
        Task<List<DTOTables>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request);
        Task<List<DTOTables>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<DTOTables?> GetDataByNameAsync(string TableNumber);
        Task<List<DTOTables>> GetAllDataAsync();
        Task<List<DTOTables>> GetAllDataAvailablesAsync();
    }

    public interface ITablesRepositoryWriter : IWritableDataBase<DTOTables,DTOTablesCRequest, DTOTablesURequest>
    { 
    }


    public interface ITablesRepositoryReadable
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

    public interface ITablesRepositoryWritable
    {
        Task<DTOTables?> AddTableAsync(DTOTablesCRequest Table);
        Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Table);
        Task<bool> DeleteTableAsync(int id);
    }



    public interface ITablesRepository : ITablesRepositoryReadable, ITablesRepositoryWritable
    { }
}

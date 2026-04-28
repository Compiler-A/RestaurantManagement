using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.Tables;


namespace DataLayerRestaurant.Interfaces
{
    public interface ITablesRepositoryReader : IReadableDataBase<Table>
    {
        Task<List<Table>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request);
        Task<List<Table>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request);
        Task<List<Table>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<Table?> GetDataByNameAsync(string TableNumber);
        Task<List<Table>> GetAllDataAsync();
        Task<List<Table>> GetAllDataAvailablesAsync();
    }

    public interface ITablesRepositoryWriter : IWritableDataBase<Table,DTOTablesCRequest, DTOTablesURequest>
    { 
    }


    public interface ITablesRepositoryReadable
    {
        Task<Table?> GetTableAsync(int id);
        Task<List<Table>> GetAlltablesAsync(int page);
        Task<List<Table>> GetFilterStatusTablesAsync(DTOTablesFilterStatusTableRequest Request);
        Task<List<Table>> GetFilterSeatTablesAsync(DTOTablesFilterSeatTableRequest Request);
        Task<List<Table>> GetFilterStatusAndSeatTablesAsync(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<Table?> GetTableByNameAsync(string TableNumber);
        Task<List<Table>> GetAlltablesAsync();
        Task<List<Table>> GetAllTablesAvailablesAsync();
    }

    public interface ITablesRepositoryWritable
    {
        Task<Table?> AddTableAsync(DTOTablesCRequest Table);
        Task<Table?> UpdateTableAsync(DTOTablesURequest Table);
        Task<bool> DeleteTableAsync(int id);
    }



    public interface ITablesRepository : ITablesRepositoryReadable, ITablesRepositoryWritable
    { }
}

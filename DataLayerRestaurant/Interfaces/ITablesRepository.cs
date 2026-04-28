using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.Tables;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Interfaces
{
    public interface ITablesRepositoryReader : IRepositoryReader<Table>
    {
        Task<List<Table>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request);
        Task<List<Table>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request);
        Task<List<Table>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request);
        Task<Table?> GetDataByNameAsync(string TableNumber);
        Task<List<Table>> GetAllDataAsync();
        Task<List<Table>> GetAllDataAvailablesAsync();
    }

    public interface ITablesRepositoryWriter : IRepositoryWriter<Table,DTOTablesCRequest, DTOTablesURequest>
    { }

    public interface ITablesRepository : ITablesRepositoryReader, ITablesRepositoryWriter
    { }
}

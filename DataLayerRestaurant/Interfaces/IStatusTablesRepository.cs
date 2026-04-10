using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.StatusTables;

namespace DataLayerRestaurant.Interfaces
{
    public interface IStatusTablesRepositoryReader : IReadableDataBase<DTOStatusTables>
    {
        Task<bool> isFindDataAsync(int id);
    }

    public interface IStatusTablesRepositoryWriter : IWritableDataBase<DTOStatusTables,DTOStatusTablesCRequest, DTOStatusTablesURequest>
    {

    }

    public interface IStatusTablesRepositoryReadable
    {
        Task<DTOStatusTables?> GetStatuTableAsync(int id);
        Task<List<DTOStatusTables>> GetAllStatustablesAsync(int page);
        Task<bool> isFindAsync(int id);
    }

    public interface IStatusTablesRepositoryWritable
    {
        Task<DTOStatusTables?> AddStatusTableAsync(DTOStatusTablesCRequest Request);
        Task<DTOStatusTables?> UpdateStatusTableAsync(DTOStatusTablesURequest Request);
        Task<bool> DeleteStatusTableAsync(int id);
    }



    public interface IStatusTablesRepository : IStatusTablesRepositoryReadable, IStatusTablesRepositoryWritable
    { }
}

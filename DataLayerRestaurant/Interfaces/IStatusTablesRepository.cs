using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.StatusTables;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IStatusTablesRepositoryReader : IReadableDataBase<StatusTable>
    {
        Task<bool> isFindDataAsync(int id);
    }

    public interface IStatusTablesRepositoryWriter : IWritableDataBase<StatusTable,DTOStatusTablesCRequest, DTOStatusTablesURequest>
    {

    }

    public interface IStatusTablesRepositoryReadable
    {
        Task<StatusTable?> GetStatuTableAsync(int id);
        Task<List<StatusTable>> GetAllStatustablesAsync(int page);
        Task<bool> isFindAsync(int id);
    }

    public interface IStatusTablesRepositoryWritable
    {
        Task<StatusTable?> AddStatusTableAsync(DTOStatusTablesCRequest Request);
        Task<StatusTable?> UpdateStatusTableAsync(DTOStatusTablesURequest Request);
        Task<bool> DeleteStatusTableAsync(int id);
    }



    public interface IStatusTablesRepository : IStatusTablesRepositoryReadable, IStatusTablesRepositoryWritable
    { }
}

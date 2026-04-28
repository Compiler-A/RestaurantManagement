using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.StatusTables;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IStatusTablesRepositoryReader : IRepositoryReader<StatusTable>
    {
        Task<bool> isFindDataAsync(int id);
    }

    public interface IStatusTablesRepositoryWriter : IRepositoryWriter<StatusTable,DTOStatusTablesCRequest, DTOStatusTablesURequest>
    {

    }

    public interface IStatusTablesRepository : IStatusTablesRepositoryReader, IStatusTablesRepositoryWriter
    { }
}

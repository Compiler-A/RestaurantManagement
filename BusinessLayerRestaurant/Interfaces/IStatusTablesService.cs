using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusTables;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IStatusTablesServiceReader : IReadableBusinessBase<DTOStatusTables>
    {   
        Task<bool> isFindAsync(int id);
    }

    public interface IStatusTablesServiceWriter : IWritableBusinessBase<DTOStatusTables, DTOStatusTablesCRequest, DTOStatusTablesURequest>
    { }


    public interface IStatusTablesServiceContainer : IInterfaceBase<IStatusTablesRepository>
    { }


    public interface IStatusTablesServiceReadable
    {
        Task<List<DTOStatusTables>> GetAllStatusTablesAsync(int page);
        Task<DTOStatusTables?> GetStatusTableAsync(int id);
        Task<bool> isFindStatusTableAsync(int id);
    }

    public interface IStatusTablesServiceWritable
    {
        Task<DTOStatusTables?> AddStatusTableAsync(DTOStatusTablesCRequest Request);
        Task<DTOStatusTables?> UpdateStatusTableAsync(DTOStatusTablesURequest Request);
        Task<bool> DeleteStatusTableAsync(int ID);

    }
    public interface ICRUDStatusTablesService : IStatusTablesServiceWritable, IStatusTablesServiceReadable
    { }

    public interface IStatusTablesService : ICRUDStatusTablesService
    { }

}

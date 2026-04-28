using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusTables;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IStatusTablesServiceReader : IReadableBusinessBase<StatusTable>
    {   
        Task<bool> isFindAsync(int id);
    }

    public interface IStatusTablesServiceWriter : IWritableBusinessBase<StatusTable, DTOStatusTablesCRequest, DTOStatusTablesURequest>
    { }


    public interface IStatusTablesServiceContainer : IInterfaceBase<IStatusTablesRepository>
    { }


    public interface IStatusTablesServiceReadable
    {
        Task<List<StatusTable>> GetAllStatusTablesAsync(int page);
        Task<StatusTable?> GetStatusTableAsync(int id);
        Task<bool> isFindStatusTableAsync(int id);
    }

    public interface IStatusTablesServiceWritable
    {
        Task<StatusTable?> AddStatusTableAsync(DTOStatusTablesCRequest Request);
        Task<StatusTable?> UpdateStatusTableAsync(DTOStatusTablesURequest Request);
        Task<bool> DeleteStatusTableAsync(int ID);

    }
    public interface ICRUDStatusTablesService : IStatusTablesServiceWritable, IStatusTablesServiceReadable
    { }

    public interface IStatusTablesService : ICRUDStatusTablesService
    { }

}

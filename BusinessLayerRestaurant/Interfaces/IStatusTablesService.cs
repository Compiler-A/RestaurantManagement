using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusTables;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IStatusTablesServiceReader : IServiceReader<StatusTable>
    {   
        Task<bool> isFindAsync(int id);
    }

    public interface IStatusTablesServiceWriter : IServiceWriter<StatusTable, DTOStatusTablesCRequest, DTOStatusTablesURequest>
    { }


    public interface IStatusTablesServiceContainer : IServiceContainer<IStatusTablesRepository>
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

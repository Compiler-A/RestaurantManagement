using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusTables;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IReadableBStatusTables : IReadableBusinessBase<DTOStatusTables>
    {   
        Task<bool> isFindAsync(int id);
    }

    public interface IWritableBStatusTables : IWritableBusinessBase<DTOStatusTables, DTOStatusTablesCRequest, DTOStatusTablesURequest>
    { }


    public interface IInterfaceBStatusTables : IInterfaceBase<IDataStatusTables>
    { }


    public interface IReadableBusinessStatusTables
    {
        Task<List<DTOStatusTables>> GetAllStatusTablesAsync(int page);
        Task<DTOStatusTables?> GetStatusTableAsync(int id);
        Task<bool> isFindStatusTableAsync(int id);
    }

    public interface IWritableBusinessStatusTables
    {
        Task<DTOStatusTables?> AddStatusTableAsync(DTOStatusTablesCRequest Request);
        Task<DTOStatusTables?> UpdateStatusTableAsync(DTOStatusTablesURequest Request);
        Task<bool> DeleteStatusTableAsync(int ID);

    }
    public interface ICRUDBusinessStatusTables : IWritableBusinessStatusTables, IReadableBusinessStatusTables
    { }

    public interface IBusinessStatusTables : ICRUDBusinessStatusTables
    { }

}

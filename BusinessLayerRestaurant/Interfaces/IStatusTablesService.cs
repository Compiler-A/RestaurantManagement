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


    public interface ICRUDStatusTablesService : IStatusTablesServiceWriter, IStatusTablesServiceReader
    { }

    public interface IStatusTablesService : ICRUDStatusTablesService
    { }

}

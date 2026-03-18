using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IDTOBTables : IDTOBase<DTOTablesCRequest, DTOTablesURequest>
    { }

    public interface IInterfaceBTables : IInterfaceBase<IDataTables>
    {
        IBusinessStatusTables IBusinessStatusTable{ get; set; }
    }

    public interface ICompositionBTables
    {
        Task LoadDataAsync(DTOTables item);
    }

    public interface IInterfaceBusinessTables
    {
        IBusinessStatusTables IStatusTable { get; set; }
    }

    public interface IReadableBTables : IReadableBusinessBase<DTOTables>
    {
        Task<List<DTOTables>> GetAllAsync();
        Task<List<DTOTables>> GetFilter1Async(int Page, int StatusTable);
        Task<List<DTOTables>> GetFilter2Async(int Page, int Seats);
        Task<DTOTables?> GetByNameAsync(string tableNumber);
        Task<List<DTOTables>> GetFilter3Async(int page, int StatusTable, int SeatNumber);
        Task<List<DTOTables>> GetAllAvailablesAsync();
    }

    public interface IWritableBTables
       : IWritableBusinessBase<DTOTables, DTOTablesCRequest, DTOTablesURequest>
    {
    }

    public interface IReadableBusinessTables
    {
        Task<List<DTOTables>> GetAllTablesAsync(int page);
        Task<DTOTables?> GetTableAsync(int id);
        Task<List<DTOTables>> GetAllTablesAsync();
        Task<List<DTOTables>> GetTablesFilter1Async(int Page, int StatusTable);
        Task<List<DTOTables>> GetTablesFilter2Async(int Page, int Seats);
        Task<DTOTables?> GetTableByNameAsync(string tableNumber);
        Task<List<DTOTables>> GetTablesFilter3Async(int page, int StatusTable, int SeatNumber);
        Task<List<DTOTables>> GetAllTablesAvailablesAsync();
    }
    public interface IWritableBusinessTables
    {
        Task<DTOTables?> AddTableAsync(DTOTablesCRequest Request);
        Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Request);
        Task<bool> DeleteTableAsync(int ID);
    }
    public interface ICRUDBusinessTables : IReadableBusinessTables, IWritableBusinessTables
    { }
    public interface IPropertiesBusinessTables : IDTOBTables, IInterfaceBusinessTables
    { }
    public interface IBusinessTables : ICRUDBusinessTables, IPropertiesBusinessTables
    {
    }
}

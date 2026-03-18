using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public interface IReadableDTables : IReadableDataBase<DTOTables>
    {
        Task<List<DTOTables>> GetFilterStatusDataAsync(int page, int StatusTable);
        Task<List<DTOTables>> GetFilterSeatDataAsync(int page, int FilterSeats);
        Task<List<DTOTables>> GetFilterStatusAndSeatDataAsync(int page, int StatusTable, int SeatNumber);
        Task<DTOTables?> GetDataByNameAsync(string TableNumber);
        Task<List<DTOTables>> GetAllDataAsync();
        Task<List<DTOTables>> GetAllDataAvailablesAsync();
    }

    public interface IWritableDTables : IWritableDataBase<DTOTables,DTOTablesCRequest, DTOTablesURequest>
    { 
    }


    public interface IReadableDataTables
    {
        Task<DTOTables?> GetTableAsync(int id);
        Task<List<DTOTables>> GetAlltablesAsync(int page);
        Task<List<DTOTables>> GetFilterStatusTablesAsync(int page, int StatusTable);
        Task<List<DTOTables>> GetFilterSeatTablesAsync(int page, int FilterSeats);
        Task<List<DTOTables>> GetFilterStatusAndSeatTablesAsync(int page, int StatusTable, int SeatNumber);
        Task<DTOTables?> GetTableByNameAsync(string TableNumber);
        Task<List<DTOTables>> GetAlltablesAsync();
        Task<List<DTOTables>> GetAllTablesAvailablesAsync();
    }

    public interface IWritableDataTables
    {
        Task<DTOTables?> AddTableAsync(DTOTablesCRequest Table);
        Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Table);
        Task<bool> DeleteTableAsync(int id);
    }



    public interface IDataTables : IReadableDataTables, IWritableDataTables
    { }
}

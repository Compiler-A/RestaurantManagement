using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public interface IReadableDStatusTables : IReadableDataBase<DTOStatusTables>
    {
        Task<bool> isFindDataAsync(int id);
    }

    public interface IWritableDStatusTables : IWritableDataBase<DTOStatusTables,DTOStatusTablesCRequest, DTOStatusTablesURequest>
    {

    }

    public interface IReadableDataStatusTables
    {
        Task<DTOStatusTables?> GetStatuTableAsync(int id);
        Task<List<DTOStatusTables>> GetAllStatustablesAsync(int page);
        Task<bool> isFindAsync(int id);
    }

    public interface IWritableDataStatusTables
    {
        Task<DTOStatusTables?> AddStatusTableAsync(DTOStatusTablesCRequest Request);
        Task<DTOStatusTables?> UpdateStatusTableAsync(DTOStatusTablesURequest Request);
        Task<bool> DeleteStatusTableAsync(int id);
    }



    public interface IDataStatusTables : IReadableDataStatusTables, IWritableDataStatusTables
    { }
}

using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IReadableBStatusTables : IReadableBusinessBase<DTOStatusTables>
    {   
        Task<bool> isFindAsync(int id);
    }

    public interface IWritableBStatusTables : IWritableBusinessBase<DTOStatusTables, DTOStatusTablesCRequest, DTOStatusTablesURequest>
    { }

    public interface IDTOBStatusTables : IDTOBase<DTOStatusTablesCRequest , DTOStatusTablesURequest>
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

    public interface IBusinessStatusTables : ICRUDBusinessStatusTables, IDTOBStatusTables
    { }

}

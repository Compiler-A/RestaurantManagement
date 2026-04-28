
using Microsoft.Data.SqlClient;

namespace RestaurantDataLayer
{
    public interface IRepositoryReader <Tentity>
    {
        Task<List<Tentity>> GetAllDataAsync(int page);
        Task<Tentity?> GetDataAsync(int ID);
    }
    public interface IRepositoryWriter <Entity,DTOCreate, DTOUpdate>
    {
        Task<Entity?> CreateDataAsync(DTOCreate dto);
        Task<Entity?> UpdateDataAsync(DTOUpdate dto);
        Task<bool> DeleteDataAsync(int id);
    }

    public interface IServiceReader <Tentity>
    {
        Task<List<Tentity>> GetAllAsync(int page);
        Task<Tentity?> GetAsync(int ID);
    }
    public interface IServiceWriter <TDTO,DTOCreate, DTOUpdate>
    {
        Task<TDTO?> CreateAsync(DTOCreate dto);
        Task<TDTO?> UpdateAsync(DTOUpdate dto);
        Task<bool> DeleteAsync(int ID);
    }

    public interface ICompositionDataBase <DTO>
    {
        DTO GetDataFromDataBase(SqlDataReader reader);
    }

    public interface IServiceContainer <DataInterface>
    {
        DataInterface IData { get; set; }
    }
}

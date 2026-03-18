using Microsoft.Data.SqlClient;

namespace RestaurantDataLayer
{
    public interface IReadableDataBase <Tentity>
    {
        Task<List<Tentity>> GetAllDataAsync(int page);
        Task<Tentity?> GetDataAsync(int ID);
    }
    public interface IWritableDataBase <DTO,DTOCreate, DTOUpdate>
    {
        Task<DTO?> CreateDataAsync(DTOCreate dto);
        Task<DTO?> UpdateDataAsync(DTOUpdate dto);
        Task<bool> DeleteDataAsync(int id);
    }

    public interface IReadableBusinessBase <Tentity>
    {
        Task<List<Tentity>> GetAllAsync(int page);
        Task<Tentity?> GetAsync(int ID);
    }
    public interface IWritableBusinessBase <TDTO,DTOCreate, DTOUpdate>
    {
        Task<TDTO?> CreateAsync(DTOCreate dto);
        Task<TDTO?> UpdateAsync(DTOUpdate dto);
        Task<bool> DeleteAsync(int ID);
    }

    public interface ICompositionDataBase <DTO>
    {
        DTO GetDataFromDataBase(SqlDataReader reader);
    }

    public interface IDTOBase <DTOcreate, DTOupdate>
    {
        DTOcreate? CreateRequest {  get; set; }
        DTOupdate? UpdateRequest { get; set; }
    }
    public interface IInterfaceBase <DataInterface>
    {
        DataInterface IData { get; set; }
    }

    public class clsDataAccessLayer
    {
        public static readonly string ConnectionString = "Server=COMPILER;Database=RestaurantManager;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static readonly int Rows = 12;
    }
}

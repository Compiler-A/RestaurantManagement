using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.JobRoles;

namespace DataLayerRestaurant.Interfaces
{
    public interface IReadableDJobRoles : IReadableDataBase<DTOJobRoles>
    { }
    public interface IWritableDJobRoles
        : IWritableDataBase<DTOJobRoles, DTOJobRolesCRequest, DTOJobRolesURequest> 
    { }

    public interface IReadableDataJobRoles
    {
        Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page);
        Task<DTOJobRoles?> GetJobRoleAsync(int ID);

    }
    public interface IWritableDataJobRoles
    {
        Task<DTOJobRoles?> AddJobRoleAsync(DTOJobRolesCRequest DTO);
        Task<DTOJobRoles?> UpdateJobRoleAsync(DTOJobRolesURequest DTO);
        Task<bool> DeleteJobRoleAsync(int ID);
    }

    public interface IDataJobRoles : IReadableDataJobRoles, IWritableDataJobRoles
    { }

}

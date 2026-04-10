using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.JobRoles;

namespace DataLayerRestaurant.Interfaces
{
    public interface IJobRolesRepositoryReader : IReadableDataBase<DTOJobRoles>
    { }
    public interface IJobRolesRepositoryWriter
        : IWritableDataBase<DTOJobRoles, DTOJobRolesCRequest, DTOJobRolesURequest> 
    { }

    public interface IJobRolesRepositoryReadable
    {
        Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page);
        Task<DTOJobRoles?> GetJobRoleAsync(int ID);

    }
    public interface IJobRolesRepositoryWritable
    {
        Task<DTOJobRoles?> AddJobRoleAsync(DTOJobRolesCRequest DTO);
        Task<DTOJobRoles?> UpdateJobRoleAsync(DTOJobRolesURequest DTO);
        Task<bool> DeleteJobRoleAsync(int ID);
    }

    public interface IJobRolesRepository : IJobRolesRepositoryReadable, IJobRolesRepositoryWritable
    { }

}

using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.JobRoles;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IJobRolesRepositoryReader : IReadableDataBase<JobRole>
    { }
    public interface IJobRolesRepositoryWriter
        : IWritableDataBase<JobRole, DTOJobRolesCRequest, DTOJobRolesURequest> 
    { }

    public interface IJobRolesRepositoryReadable
    {
        Task<List<JobRole>> GetAllJobRolesAsync(int page);
        Task<JobRole?> GetJobRoleAsync(int ID);

    }
    public interface IJobRolesRepositoryWritable
    {
        Task<JobRole?> AddJobRoleAsync(DTOJobRolesCRequest DTO);
        Task<JobRole?> UpdateJobRoleAsync(DTOJobRolesURequest DTO);
        Task<bool> DeleteJobRoleAsync(int ID);
    }

    public interface IJobRolesRepository : IJobRolesRepositoryReadable, IJobRolesRepositoryWritable
    { }

}

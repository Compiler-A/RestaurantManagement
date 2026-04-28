using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.JobRoles;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
{


    public interface IJobRolesServiceContainer : IInterfaceBase<IJobRolesRepository>
    { }

    public interface IJobRolesServiceReader : IReadableBusinessBase<JobRole> 
    { }
    public interface IJobRolesServiceWriter : IWritableBusinessBase<JobRole, DTOJobRolesCRequest, DTOJobRolesURequest>
    { }

    public interface IJobRolesServiceReadable
    {
        Task<List<JobRole>> GetAllJobRolesAsync(int page);
        Task<JobRole?> GetJobRoleAsync(int ID);
    }

    public interface IJobRolesServiceWritable
    {
        Task<JobRole?> AddJobRoleAsync(DTOJobRolesCRequest Request);
        Task<JobRole?> UpdateJobRoleAsync(DTOJobRolesURequest Request);
        Task<bool> DeleteJobRoleAsync(int ID);
    }

    public interface ICRUDJobRolesService : IJobRolesServiceReadable, IJobRolesServiceWritable
    { }

    public interface IJobRolesService : ICRUDJobRolesService
    {
    }
}

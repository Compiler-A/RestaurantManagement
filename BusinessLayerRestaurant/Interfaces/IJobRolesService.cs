using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.JobRoles;

namespace BusinessLayerRestaurant.Interfaces
{


    public interface IJobRolesServiceContainer : IInterfaceBase<IJobRolesRepository>
    { }

    public interface IJobRolesServiceReader : IReadableBusinessBase<DTOJobRoles> 
    { }
    public interface IJobRolesServiceWriter : IWritableBusinessBase<DTOJobRoles, DTOJobRolesCRequest, DTOJobRolesURequest>
    { }

    public interface IJobRolesServiceReadable
    {
        Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page);
        Task<DTOJobRoles?> GetJobRoleAsync(int ID);
    }

    public interface IJobRolesServiceWritable
    {
        Task<DTOJobRoles?> AddJobRoleAsync(DTOJobRolesCRequest Request);
        Task<DTOJobRoles?> UpdateJobRoleAsync(DTOJobRolesURequest Request);
        Task<bool> DeleteJobRoleAsync(int ID);
    }

    public interface ICRUDJobRolesService : IJobRolesServiceReadable, IJobRolesServiceWritable
    { }

    public interface IJobRolesService : ICRUDJobRolesService
    {
    }
}

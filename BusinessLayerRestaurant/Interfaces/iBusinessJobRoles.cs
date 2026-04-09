using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.JobRoles;

namespace BusinessLayerRestaurant.Interfaces
{


    public interface IInterfaceBJobRoles : IInterfaceBase<IDataJobRoles>
    { }

    public interface IReadableBJobRoles : IReadableBusinessBase<DTOJobRoles> 
    { }
    public interface IWritableBJobRoles : IWritableBusinessBase<DTOJobRoles, DTOJobRolesCRequest, DTOJobRolesURequest>
    { }

    public interface IReadableBusinessJobRoles
    {
        Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page);
        Task<DTOJobRoles?> GetJobRoleAsync(int ID);
    }

    public interface IWritableBusinessJobRoles
    {
        Task<DTOJobRoles?> AddJobRoleAsync(DTOJobRolesCRequest Request);
        Task<DTOJobRoles?> UpdateJobRoleAsync(DTOJobRolesURequest Request);
        Task<bool> DeleteJobRoleAsync(int ID);
    }

    public interface ICRUDBusinessJobRoles : IReadableBusinessJobRoles, IWritableBusinessJobRoles
    { }

    public interface IBusinessJobRoles : ICRUDBusinessJobRoles
    {
    }
}

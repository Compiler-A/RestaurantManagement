using ContractsLayerRestaurant.DTORequest.JobRoles;
using DomainLayer.Entities;

namespace ContractsLayerRestaurant.Interfaces.Repositories
{
    public interface IJobRolesRepositoryReader : IRepositoryReader<JobRole>
    {
    }
    public interface IJobRolesRepositoryWriter
        : IRepositoryWriter<JobRole, DTOJobRolesCRequest, DTOJobRolesURequest> 
    { }

    public interface IJobRolesRepository : IJobRolesRepositoryReader, IJobRolesRepositoryWriter
    { }

}

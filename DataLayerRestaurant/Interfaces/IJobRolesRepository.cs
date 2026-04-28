using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.JobRoles;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IJobRolesRepositoryReader : IRepositoryReader<JobRole>
    { }
    public interface IJobRolesRepositoryWriter
        : IRepositoryWriter<JobRole, DTOJobRolesCRequest, DTOJobRolesURequest> 
    { }

    public interface IJobRolesRepository : IJobRolesRepositoryReader, IJobRolesRepositoryWriter
    { }

}

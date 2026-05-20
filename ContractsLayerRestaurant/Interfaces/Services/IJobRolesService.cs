using ContractsLayerRestaurant.Interfaces.Repositories;
using ContractsLayerRestaurant.DTORequest.JobRoles;
using DomainLayer.Entities;

namespace ContractsLayerRestaurant.Interfaces.Services
{


    public interface IJobRolesServiceContainer : IServiceContainer<IJobRolesRepository>
    { }

    public interface IJobRolesServiceReader : IServiceReader<JobRole> 
    { }
    public interface IJobRolesServiceWriter : IServiceWriter<JobRole, DTOJobRolesCRequest, DTOJobRolesURequest>
    { }


    public interface ICRUDJobRolesService : IJobRolesServiceReader, IJobRolesServiceWriter
    { }

    public interface IJobRolesService : ICRUDJobRolesService
    {
    }
}

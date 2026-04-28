using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.JobRoles;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Interfaces
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

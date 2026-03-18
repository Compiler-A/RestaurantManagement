using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IDTOBJobRoles : IDTOBase<DTOJobRolesCRequest, DTOJobRolesURequest>
    {
    }

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

    public interface IBusinessJobRoles : ICRUDBusinessJobRoles, IDTOBJobRoles
    {
    }
}

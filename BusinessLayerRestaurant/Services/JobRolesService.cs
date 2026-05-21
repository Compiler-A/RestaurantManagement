using ContractsLayerRestaurant.DTORequest.JobRoles;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{

    public class JobRolesService : IJobRolesService
    {

        IJobRolesServiceReader _IRead;
        IJobRolesServiceWriter _IWrite;
        public JobRolesService(IJobRolesServiceReader iRead, IJobRolesServiceWriter iWrite)
        {
            _IRead = iRead;
            _IWrite = iWrite;
        }

        public async Task<List<JobRole>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<JobRole?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<JobRole?> CreateAsync(DTOJobRolesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<JobRole?> UpdateAsync(DTOJobRolesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

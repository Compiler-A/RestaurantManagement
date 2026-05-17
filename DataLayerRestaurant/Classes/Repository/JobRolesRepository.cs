using ContractsLayerRestaurant.DTORequest.JobRoles;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Classes.Repository
{

    public class JobRolesRepository : IJobRolesRepository
    {
        IJobRolesRepositoryWriter _IWrite;
        IJobRolesRepositoryReader _IRead;

        public JobRolesRepository(IJobRolesRepositoryWriter write, IJobRolesRepositoryReader read)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<JobRole>> GetAllDataAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }

        public async Task<List<JobRole>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<JobRole?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<JobRole?> CreateDataAsync(DTOJobRolesCRequest DTO)
        {
            return await _IWrite.CreateDataAsync(DTO);
        }

        public async Task<JobRole?> UpdateDataAsync(DTOJobRolesURequest DTO)
        {
            return await _IWrite.UpdateDataAsync(DTO);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }
}

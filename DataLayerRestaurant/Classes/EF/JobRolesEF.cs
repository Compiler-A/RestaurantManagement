

using ContractsLayerRestaurant.DTORequest.JobRoles;
using DataLayerRestaurant.Data;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.EF
{
    public class JobRolesRepositoryReaderEF : IJobRolesRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;
        public JobRolesRepositoryReaderEF(IOptions<clsMySettings> mySettings, AppDBContext dbContext)
        {
            _Settings = mySettings.Value;
            _DbContext = dbContext;
        }


        public async Task<List<JobRole>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.JobRoles
                .AsNoTracking()
                .Where(x => Ids.Contains(x.JobRoleID));
            return await query.ToListAsync();
        }

        public async Task<List<JobRole>> GetAllDataAsync(int page)
        {
            var query = _DbContext.JobRoles
                .AsNoTracking()
                .OrderBy(x => x.JobRoleID);
                
            var data = query.Skip((page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }

        public async Task<JobRole?> GetDataAsync(int ID)
        {
            var query = _DbContext.JobRoles
                .AsNoTracking();


            var data = query.Where(x => x.JobRoleID == ID);

            return await data.FirstOrDefaultAsync();
        }
    }

    public class JobRolesRepositoryWriterEF : IJobRolesRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;
        public JobRolesRepositoryWriterEF(IOptions<clsMySettings> mySettings, AppDBContext dbContext)
        {
            _Settings = mySettings.Value;
            _DbContext = dbContext;
        }

        public async Task<JobRole?> CreateDataAsync(DTOJobRolesCRequest DTO)
        {
            var Entity = new JobRole
            {
                JobName = DTO.Name,
                Description = DTO.Description
            };


            await _DbContext.JobRoles.AddAsync(Entity);
            await _DbContext.SaveChangesAsync();

            return Entity;
        }

        public async Task<JobRole?> UpdateDataAsync(DTOJobRolesURequest DTO)
        {
            var Entity = await _DbContext.JobRoles.FindAsync(DTO.ID);
            if (Entity != null)
            {
                Entity.JobName = DTO.Name;
                Entity.Description = DTO.Description;
                await _DbContext.SaveChangesAsync();
            }
            return Entity;
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            var Entity = await _DbContext.JobRoles.FindAsync(ID);
            if (Entity == null)
            {
                return false;
            }

            _DbContext.JobRoles.Remove(Entity);
            return await _DbContext.SaveChangesAsync() > 0;
        }
    }
}

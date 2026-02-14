using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public interface IReadableJobRolesBusiness
    {
        Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page);
        Task<DTOJobRoles?> GetJobRolesAsync(int ID);
    }

    public interface IWritableJobRolesBusiness
    {
        Task<bool> Save();
        Task<bool> Delete(int ID);
    }

    public interface IBusinessJobRoles : IReadableJobRolesBusiness, IWritableJobRolesBusiness
    { 
        DTOJobRoles? JobRoles { get; set; }
    }

    public class clsBusinessJobRoles : IBusinessJobRoles
    {
        public enum enMode
        {
            add, update
        }

        public enMode Mode { get; set; } = enMode.add;

        private DTOJobRoles? _jobRoles { get; set; }
        public DTOJobRoles? JobRoles
        {
            get => _jobRoles;
            set => _jobRoles = value;
        }

        public IDataJobRoles dataJobRoles;

        public clsBusinessJobRoles(IDataJobRoles job)
        {
            dataJobRoles = job;
            Mode = enMode.add;
        }

        public async Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page)
        {
            return await dataJobRoles.GetAllJobRoles(page);
        }

        public async Task<DTOJobRoles?> GetJobRolesAsync(int ID)
        {
            var dto = await dataJobRoles.GetJobRole(ID);
            if (dto == null)
            {
                return null;
            }
            _jobRoles = dto;
            Mode = enMode.update;
            return dto;
        }

        private async Task<bool> _Add()
        {
            _jobRoles!.ID = await dataJobRoles.Add(_jobRoles);
            if (_jobRoles!.ID != -1)
            {
                Mode = enMode.update;
                return true;
            }
            return false;
        }

        private async Task<bool> _Update()
        {
            return await dataJobRoles.Update(_jobRoles!);
        }

        public async Task<bool> Save()
        {
            if (_jobRoles == null)
            {
                return false;
            }
            return Mode switch
            {
                enMode.add => await _Add(),
                enMode.update => await _Update(),
                _ => false
            };
        }

        public async Task<bool> Delete(int ID)
        {
            return await dataJobRoles.Delete(ID);
        }
    }
}

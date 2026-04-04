using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerRestaurant;

namespace BusinessLayerRestaurant
{
    public class clsJobRolesDtoContainer : IDTOBJobRoles
    {
        private DTOJobRolesCRequest? _CRequest;
        public DTOJobRolesCRequest? CreateRequest
        {
            get => _CRequest;
            set => _CRequest = value;
        }

        private DTOJobRolesURequest? _URequest;
        public DTOJobRolesURequest? UpdateRequest
        {
            get => _URequest;
            set => _URequest = value;
        }
    }

    public class clsBJobRolesRepositoryBridge : IInterfaceBJobRoles
    {
        private IDataJobRoles _IJobRole;
        public IDataJobRoles IData
        {
            get => _IJobRole;
            set => _IJobRole = value;
        }

        public clsBJobRolesRepositoryBridge(IDataJobRoles IJobRole)
        {
            _IJobRole = IJobRole;
        }

    }


    public class clsJobRolesReader : IReadableBJobRoles
    {
        private IInterfaceBJobRoles _Interface;
        public clsJobRolesReader(IInterfaceBJobRoles setting)
        {
            _Interface = setting;
        }

        public async Task<List<DTOJobRoles>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllJobRolesAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return list;
        }
        public async Task<DTOJobRoles?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetJobRoleAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return dto;
        }
    }

    public class clsJobRolesWriter :  IWritableBJobRoles
    {
        private IInterfaceBJobRoles _Interface;
        public clsJobRolesWriter(IInterfaceBJobRoles jobrole)
        {
            _Interface = jobrole;
        }



        public async Task<DTOJobRoles?> CreateAsync(DTOJobRolesCRequest Request)
        {

            var dto = await _Interface.IData.AddJobRoleAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            return dto;
        }
        public async Task<DTOJobRoles?> UpdateAsync(DTOJobRolesURequest Request)
        {
            var dto = await _Interface.IData.UpdateJobRoleAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Update!");
            }
            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteJobRoleAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Delete!");
            }
            return result;
        }
    }

   

    public class clsBusinessJobRoles : IBusinessJobRoles
    {

        IDTOBJobRoles _IProperties;
        IReadableBJobRoles _IRead;
        IWritableBJobRoles _IWrite;
        public clsBusinessJobRoles(IDTOBJobRoles dto, IReadableBJobRoles iRead, IWritableBJobRoles iWrite)
        {
            _IProperties = dto;
            _IRead = iRead;
            _IWrite = iWrite;
        }

        public DTOJobRolesCRequest? CreateRequest
        {
            get => _IProperties.CreateRequest;
            set => _IProperties.CreateRequest = value;
        }

        public DTOJobRolesURequest? UpdateRequest
        {
            get => _IProperties.UpdateRequest;
            set => _IProperties.UpdateRequest = value;
        }

        public async Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<DTOJobRoles?> GetJobRoleAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<DTOJobRoles?> AddJobRoleAsync(DTOJobRolesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOJobRoles?> UpdateJobRoleAsync(DTOJobRolesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteJobRoleAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

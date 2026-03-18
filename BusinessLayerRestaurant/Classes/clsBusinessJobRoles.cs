using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerRestaurant;
using Microsoft.Identity.Client;

namespace BusinessLayerRestaurant
{
    public class clsDTOBJobRoles : IDTOBJobRoles
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

    public class clsInterfaceBJobRoles : IInterfaceBJobRoles
    {
        private IDataJobRoles _IJobRole;
        public IDataJobRoles IData
        {
            get => _IJobRole;
            set => _IJobRole = value;
        }

        public clsInterfaceBJobRoles(IDataJobRoles IJobRole)
        {
            _IJobRole = IJobRole;
        }

    }


    public class clsReadableBJobRoles : IReadableBJobRoles
    {
        private IInterfaceBJobRoles _Interface;
        public clsReadableBJobRoles(IInterfaceBJobRoles setting)
        {
            _Interface = setting;
        }
        public async Task<List<DTOJobRoles>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllJobRolesAsync(page);
            return list;
        }
        public async Task<DTOJobRoles?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetJobRoleAsync(ID);

            return dto;
        }
    }

    public class clsWritableBJobRoles :  IWritableBJobRoles
    {
        private IInterfaceBJobRoles _Interface;
        public clsWritableBJobRoles(IInterfaceBJobRoles jobrole)
        {
            _Interface = jobrole;
        }



        public async Task<DTOJobRoles?> CreateAsync(DTOJobRolesCRequest Request)
        {
            if (Request == null)
            { return null; }
            var dto = await _Interface.IData.AddJobRoleAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }
        public async Task<DTOJobRoles?> UpdateAsync(DTOJobRolesURequest Request)
        {
            if (Request == null || Request.ID <= 0)
            { return null; }

            var dto = await _Interface.IData.UpdateJobRoleAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interface.IData.DeleteJobRoleAsync(ID);
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

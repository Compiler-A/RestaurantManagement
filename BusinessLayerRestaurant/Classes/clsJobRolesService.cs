#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.JobRoles;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Classes
{
    

    public class clsJobRolesContainer : IJobRolesServiceContainer
    {
        private IJobRolesRepository _IJobRole;
        public IJobRolesRepository IData
        {
            get => _IJobRole;
            set => _IJobRole = value;
        }

        public clsJobRolesContainer(IJobRolesRepository IJobRole)
        {
            _IJobRole = IJobRole;
        }

    }


    public class clsJobRolesReader : IJobRolesServiceReader
    {
        private IJobRolesServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsJobRolesReader(IJobRolesServiceContainer setting, IMyLogger myLogger)
        {
            _Interface = setting;
            _Logger = myLogger;
        }

        public async Task<List<JobRole>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllDataAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Jobs Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }
        public async Task<JobRole?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetDataAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Job Found, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }
    }

    public class clsJobRolesWriter :  IJobRolesServiceWriter
    {
        private IJobRolesServiceContainer _Interface;
        private IMyLogger _Logger;
        public clsJobRolesWriter(IJobRolesServiceContainer jobrole, IMyLogger logger)
        {
            _Interface = jobrole;
            _Logger = logger;
        }



        public async Task<JobRole?> CreateAsync(DTOJobRolesCRequest Request)
        {

            var dto = await _Interface.IData.CreateDataAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"Job Created, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }
        public async Task<JobRole?> UpdateAsync(DTOJobRolesURequest Request)
        {
            var dto = await _Interface.IData.UpdateDataAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Update!");
            }
            _Logger.EventLogs($"Job Updated, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteDataAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Delete!");
            }
            _Logger.EventLogs($"Job Deleted, ID: {ID}", EventLogEntryType.Information);
            return result;
        }
    }

   

    public class clsJobRolesService : IJobRolesService
    {

        IJobRolesServiceReader _IRead;
        IJobRolesServiceWriter _IWrite;
        public clsJobRolesService( IJobRolesServiceReader iRead, IJobRolesServiceWriter iWrite)
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

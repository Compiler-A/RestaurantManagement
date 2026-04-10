#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.JobRoles;


namespace BusinessLayerRestaurant.Classes
{
    

    public class clsJobRolesContainer : IJobRolesServiceContainer
    {
        private IDataJobRoles _IJobRole;
        public IDataJobRoles IData
        {
            get => _IJobRole;
            set => _IJobRole = value;
        }

        public clsJobRolesContainer(IDataJobRoles IJobRole)
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

        public async Task<List<DTOJobRoles>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllJobRolesAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Jobs Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }
        public async Task<DTOJobRoles?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetJobRoleAsync(ID);
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



        public async Task<DTOJobRoles?> CreateAsync(DTOJobRolesCRequest Request)
        {

            var dto = await _Interface.IData.AddJobRoleAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"Job Created, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }
        public async Task<DTOJobRoles?> UpdateAsync(DTOJobRolesURequest Request)
        {
            var dto = await _Interface.IData.UpdateJobRoleAsync(Request);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Update!");
            }
            _Logger.EventLogs($"Job Updated, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteJobRoleAsync(ID);
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

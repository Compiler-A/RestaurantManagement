#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Employees;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Classes
{
    

    public class EmployeesContainer : IEmployeesServiceContainer
    {
        private IEmployeesRepository _IEmployees;
        public IEmployeesRepository IData
        {
            get => _IEmployees;
            set => _IEmployees = value;
        }

        private IJobRolesService _IJobRoles;
        public IJobRolesService IBusinessJobRole
        {
            get => _IJobRoles;
            set => _IJobRoles = value;
        }

        public EmployeesContainer(IEmployeesRepository Employee, IJobRolesService JobRoles)
        {
            _IEmployees = Employee;
            _IJobRoles = JobRoles;
        }
    }


    public class EmployeesReader :  IEmployeesServiceReader
    {
        private IEmployeesServiceContainer _Interface;
        private readonly IMyLogger _Logger;
        private readonly IHashingService _HashingService;

        public EmployeesReader(IHashingService HashingService, IEmployeesServiceContainer Interface, IMyLogger Logger) 
        {
            _Interface = Interface;
            _HashingService = HashingService;
            _Logger = Logger;
        }
        

        public async Task<Employee?> GetAsync(int ID)
        {
            var result = await _Interface.IData.GetDataAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Employee Found, UserName: {result.Username}", EventLogEntryType.Information);

            return result;
        }

        public async Task<Employee?> GetAsync(string UserName)
        {
            var dto = await _Interface.IData.GetDataAsync(UserName);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Employee Found, UserName: {dto.Username}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<List<Employee>> GetAllAsync(int page)
        {
            var result = await _Interface.IData.GetAllDataAsync(page);

            if (result == null || result.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Employees Found, Count: {result.Count}", EventLogEntryType.Information);
            return result;

        }

    }

    public class EmployeesWriter : IEmployeesServiceWriter
    {
        private IEmployeesServiceContainer _Interface;
        private IHashingService _HashingService;
        private readonly IMyLogger _Logger;
        public EmployeesWriter(IHashingService HashingService,IEmployeesServiceContainer Interface, IMyLogger Logger)
        {
            _Interface = Interface;
            _HashingService = HashingService;
            _Logger = Logger;
        }

        public async Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Request)
        {
            var Data = await _Interface.IData.GetDataAsync(Request.ID);
            if (Data == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            var Valid = _HashingService.ValidationBCrypt(Request.CurrentPassword, Data.Password);
            if (!Valid)
            {
                throw new InvalidOperationException("Bad password!");
            }

            Request.NewPassword = _HashingService.BCryptHashing(Request.NewPassword);

            var result = await _Interface.IData.ChangedDataPasswordAsync(Request);
            if (!result)
            {
                throw new InvalidOperationException("Not Changed!");
            }
            _Logger.EventLogs($"Changed Password Saccessfully.", EventLogEntryType.Information);
            return result;
        }

        public async Task<Employee?> CreateAsync(DTOEmployeesCRequest request)
        {
            request.Password = _HashingService.BCryptHashing(request.Password);

            var result = await _Interface.IData.CreateDataAsync(request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"Employee Created, UserName: {result.Username}", EventLogEntryType.Information);
            return result;
        }

        public async Task<Employee?> UpdateAsync(DTOEmployeesURequest request)
        {

            request.Password = _HashingService.BCryptHashing(request.Password);

            var result = await _Interface.IData.UpdateDataAsync(request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"Employees Updated, UserName: {result.Username}", EventLogEntryType.Information);

            return result;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteDataAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"Employees Deleted, ID: {ID}", EventLogEntryType.Information);
            return result;
        }
    }

    public class EmployeesService : IEmployeesService
    {
        IEmployeesServiceReader _IRead;
        IEmployeesServiceWriter _IWrite;
        IEmployeesServiceContainer _Interface;

        public EmployeesService
            (IEmployeesServiceReader Read, IEmployeesServiceWriter Write, IEmployeesServiceContainer Interface)
        {
            _IRead = Read;
            _IWrite = Write;
            _Interface = Interface;
        }

        public IJobRolesService IJobRole
        {
            get => _Interface.IBusinessJobRole;
            set => _Interface.IBusinessJobRole = value;
        }

        public async Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Request)
        {
            return await _IWrite.ChangePasswordAsync(Request);
        }
        

        public async Task<Employee?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }

        public async Task<Employee?> GetAsync(string Name)
        {
            return await _IRead.GetAsync(Name);
        }

        public async Task<List<Employee>> GetAllAsync(int Page)
        {
           return await _IRead.GetAllAsync(Page);
        }

        public async Task<Employee?> CreateAsync(DTOEmployeesCRequest request)
        {
            return await _IWrite.CreateAsync(request);
        }

        public async Task<Employee?> UpdateAsync(DTOEmployeesURequest request)
        {
            return await _IWrite.UpdateAsync(request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

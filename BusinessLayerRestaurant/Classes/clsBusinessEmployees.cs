#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Employees;

namespace BusinessLayerRestaurant.Classes
{
    

    public class clsEmployeesRepositoryBridge : IInterfaceBEmployees
    {
        private IDataEmployees _IEmployees;
        public IDataEmployees IData
        {
            get => _IEmployees;
            set => _IEmployees = value;
        }

        private IBusinessJobRoles _IJobRoles;
        public IBusinessJobRoles IBusinessJobRole
        {
            get => _IJobRoles;
            set => _IJobRoles = value;
        }

        public clsEmployeesRepositoryBridge(IDataEmployees Employee, IBusinessJobRoles JobRoles)
        {
            _IEmployees = Employee;
            _IJobRoles = JobRoles;
        }
    }

    public class clsJobRoleLoader : ICompositionBEmployees
    {
        private IBusinessJobRoles _IData;

        public clsJobRoleLoader(IBusinessJobRoles JobRole)
        {
            _IData = JobRole;
        }

        public async Task LoadDataAsync(DTOEmployees item)
        {
            item.JobRoles = await _IData.GetJobRoleAsync(item.JobID);
        }

    }

    public class clsCompositionEmployeeesLoader: ICompositionBEmployees
    {
        private IEnumerable<ICompositionBEmployees> _loaders;
        public clsCompositionEmployeeesLoader
            (IEnumerable<ICompositionBEmployees> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(DTOEmployees item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsEmployeesReader :  clsCompositionEmployeeesLoader ,IReadableBEmployees
    {
        private IInterfaceBEmployees _Interface;
        private readonly IMyLogger _Logger;

        public clsEmployeesReader(IInterfaceBEmployees Interface, IEnumerable<ICompositionBEmployees> Loaders, IMyLogger Logger) 
            : base(Loaders)
        {
            _Interface = Interface;
            _Logger = Logger;
        }
        public async Task<DTOEmployees?> LoginAsync(DTOEmployeesLoginRequest Request)
        {
            var dto =  await _Interface.IData.GetLoginEmployeeAsync(Request);

            if (dto == null)
            {
                throw new InvalidOperationException("Not Login!");
            }
            await LoadDataAsync(dto);
            _Logger.EventLogs($"Employee Login, UserName: {dto.UserName}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<DTOEmployees?> GetAsync(int ID)
        {
            var result = await _Interface.IData.GetEmployeeAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            await LoadDataAsync(result);
            _Logger.EventLogs($"Employee Found, UserName: {result.UserName}", EventLogEntryType.Information);

            return result;
        }

        public async Task<DTOEmployees?> GetAsync(string UserName)
        {
            var dto = await _Interface.IData.GetEmployeeAsync(UserName);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            await LoadDataAsync(dto);
            _Logger.EventLogs($"Employee Found, UserName: {dto.UserName}", EventLogEntryType.Information);
            return dto;
        }

        public async Task<List<DTOEmployees>> GetAllAsync(int page)
        {
            var result = await _Interface.IData.GetAllEmployeesAsync(page);

            if (result == null || result.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            foreach (var item in result)
            {
                await LoadDataAsync(item);
            }
            _Logger.EventLogs($"Employees Found, Count: {result.Count}", EventLogEntryType.Information);
            return result;

        }

    }

    public class clsEmployeesWriter : clsCompositionEmployeeesLoader , IWritableBEmployees
    {
        private IInterfaceBEmployees _Interface;
        private readonly IMyLogger _Logger;
        public clsEmployeesWriter(IInterfaceBEmployees Interface, IMyLogger Logger, IEnumerable<ICompositionBEmployees> Loaders)
            : base(Loaders)
        {
            _Interface = Interface;
            _Logger = Logger;
        }

        public async Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Request)
        {
            var result = await _Interface.IData.ChangedPasswordEmployeeAsync(Request);
            if (!result)
            {
                throw new InvalidOperationException("Not Changed!");
            }
            _Logger.EventLogs($"Changed Password Saccessfully.", EventLogEntryType.Information);
            return result;
        }

        public async Task<DTOEmployees?> CreateAsync(DTOEmployeesCRequest request)
        {
            var result = await _Interface.IData.AddEmployeeAsync(request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            await LoadDataAsync(result);
            _Logger.EventLogs($"Employee Created, UserName: {result.UserName}", EventLogEntryType.Information);
            return result;
        }

        public async Task<DTOEmployees?> UpdateAsync(DTOEmployeesURequest request)
        {
            

            var result = await _Interface.IData.UpdateEmployeeAsync(request);
            if (result == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"Employees Updated, UserName: {result.UserName}", EventLogEntryType.Information);

            await LoadDataAsync(result);
            return result;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteEmployeeAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"Employees Deleted, ID: {ID}", EventLogEntryType.Information);
            return result;
        }
    }

    public class clsBusinessEmployees : IBusinessEmployees
    {
        IReadableBEmployees _IRead;
        IWritableBEmployees _IWrite;
        IInterfaceBEmployees _Interface;

        public clsBusinessEmployees
            (IReadableBEmployees Read, IWritableBEmployees Write, IInterfaceBEmployees Interface)
        {
            _IRead = Read;
            _IWrite = Write;
            _Interface = Interface;
        }

        public IBusinessJobRoles IJobRole
        {
            get => _Interface.IBusinessJobRole;
            set => _Interface.IBusinessJobRole = value;
        }



        public async Task<DTOEmployees?> GetLoginEmployeeAsync(DTOEmployeesLoginRequest request)
        {
            return await _IRead.LoginAsync(request);
        }

        public async Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Request)
        {
            return await _IWrite.ChangePasswordAsync(Request);
        }
        

        public async Task<DTOEmployees?> GetEmployeeAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }

        public async Task<DTOEmployees?> GetEmployeeAsync(string Name)
        {
            return await _IRead.GetAsync(Name);
        }

        public async Task<List<DTOEmployees>> GetAllEmployeesAsync(int Page)
        {
           return await _IRead.GetAllAsync(Page);
        }

        public async Task<DTOEmployees?> CreateEmployeeAsync(DTOEmployeesCRequest request)
        {
            return await _IWrite.CreateAsync(request);
        }

        public async Task<DTOEmployees?> UpdateEmployeeAsync(DTOEmployeesURequest request)
        {
            return await _IWrite.UpdateAsync(request);
        }

        public async Task<bool> DeleteEmployeeAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

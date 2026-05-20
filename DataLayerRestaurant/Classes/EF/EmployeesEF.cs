using ContractsLayerRestaurant.DTORequest.Employees;
using DataLayerRestaurant.Data;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;


namespace DataLayerRestaurant.Classes.EF
{
    public class EmployeesRepositoryReaderEF : IEmployeesRepositoryReader
    {
        private readonly clsMySettings _Setting;
        private readonly AppDBContext _DbContext;
        public EmployeesRepositoryReaderEF(IOptions<clsMySettings> settings, AppDBContext DbContext)
        {
            _DbContext = DbContext;
            _Setting = settings.Value;
        }
        public async Task<List<Employee>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.Employees
                .AsNoTracking()
                .Where(x => Ids.Contains(x.EmployeeID))
                .Select(x => new Employee
                {
                    EmployeeID = x.EmployeeID,
                    Name = x.Name,
                    JobRoleID = x.JobRoleID,
                    Username = x.Username,
                    JobRole = new JobRole
                    {
                        JobRoleID = x.JobRole.JobRoleID,
                        JobName = x.JobRole.JobName,
                    }
                });

            return await query.ToListAsync();
        }

        public async Task<Employee?> GetDataAsync(string UserName)
        {
            var query = _DbContext.Employees
                .AsNoTracking()
                .Where(x => x.Username == UserName)
                .Select(x => new Employee
                {
                    EmployeeID = x.EmployeeID,
                    Name = x.Name,
                    JobRoleID = x.JobRoleID,
                    Username = x.Username,
                    Password = x.Password,
                    JobRole = new JobRole
                    {
                        JobRoleID = x.JobRole.JobRoleID,
                        JobName = x.JobRole.JobName,
                    }
                });

            return await query.FirstOrDefaultAsync();
        }


        public async Task<List<Employee>> GetAllDataAsync(int page)
        {
            var query = _DbContext.Employees
                .AsNoTracking()
                .OrderBy(x => x.EmployeeID)
                .Select(x => new Employee
                {
                    EmployeeID = x.EmployeeID,
                    Name = x.Name,
                    JobRoleID = x.JobRoleID,
                    Username = x.Username,
                    JobRole = new JobRole
                    {
                        JobRoleID = x.JobRole.JobRoleID,
                        JobName = x.JobRole.JobName,
                    }
                });

            var data = query.Skip((page - 1) * _Setting.RowsPerPage)
                            .Take(_Setting.RowsPerPage);

            return await data.ToListAsync();
        }

        public async Task<Employee?> GetDataAsync(int ID)
        { 
            var query = _DbContext.Employees
                .AsNoTracking()
                .Where(x => x.EmployeeID == ID)
                .Select(x => new Employee
                {
                    EmployeeID = x.EmployeeID,
                    Name = x.Name,
                    JobRoleID = x.JobRoleID,
                    Username = x.Username,
                    Password = x.Password,
                    JobRole = new JobRole
                    {
                        JobRoleID = x.JobRole.JobRoleID,
                        JobName = x.JobRole.JobName,
                    }
                });

            return await query.FirstOrDefaultAsync();
        }
    }


    public class EmployeesRepositoryWriterEF : IEmployeesRepositoryWriter
    {
        private readonly AppDBContext _DbContext;

        public EmployeesRepositoryWriterEF(AppDBContext DbContext)
        {
            _DbContext = DbContext;
            
        }

        public async Task<bool> ChangedDataPasswordAsync(DTOEmployeesChangedPassword clsChanged)
        {
            var existingEmployee = await _DbContext.Employees.FindAsync(clsChanged.ID);
            if (existingEmployee == null)
            {
                return false;
            }

            existingEmployee.Password = clsChanged.NewPassword;
            return await _DbContext.SaveChangesAsync() > 0;
        }

        public async Task<Employee?> CreateDataAsync(DTOEmployeesCRequest employee)
        {
            var jobRole = await _DbContext.JobRoles.FindAsync(employee.JobID);
            if (jobRole == null) return null;

            var newEmployee = new Employee
            {
                JobRoleID = employee.JobID,
                Name = employee.Name,
                Username = employee.UserName,
                Password = employee.Password
            };

            await _DbContext.Employees.AddAsync(newEmployee);
            await _DbContext.SaveChangesAsync();

            newEmployee.JobRole = jobRole;
            return newEmployee;
        }

        public async Task<Employee?> UpdateDataAsync(DTOEmployeesURequest employee)
        {
            var jobRole = await _DbContext.JobRoles.FindAsync(employee.JobID);
            if (jobRole == null) return null;

            var existingEmployee = await _DbContext.Employees.FindAsync(employee.ID);
            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.JobRoleID = employee.JobID;
            existingEmployee.Username = employee.UserName;
            existingEmployee.Password = employee.Password;

            await _DbContext.SaveChangesAsync();

            existingEmployee.JobRole = jobRole;
            return existingEmployee;
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            var RemoveEmployee = await _DbContext.Employees.FindAsync(ID);
            if (RemoveEmployee == null)
            {
                return false;
            }

            _DbContext.Employees.Remove(RemoveEmployee);

            int AffectRows = await _DbContext.SaveChangesAsync();
            return AffectRows > 0;
        }

    }   
}

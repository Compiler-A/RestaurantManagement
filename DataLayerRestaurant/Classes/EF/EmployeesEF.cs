using ContractsLayerRestaurant.DTORequest.Employees;
using DataLayerRestaurant.Data;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Net.WebSockets;


namespace DataLayerRestaurant.Classes.EF
{
    public class EmployeesRepositoryReaderEF : IEmployeesRepositoryReader
    {
        private readonly clsMySettings _Setting;
        private readonly AppDBContext _DbContext;
        private readonly ILogger<EmployeesRepositoryReaderEF> _Logger;
        public EmployeesRepositoryReaderEF(IOptions<clsMySettings> settings, AppDBContext DbContext, ILogger<EmployeesRepositoryReaderEF> logger)
        {
            _DbContext = DbContext;
            _Logger = logger;
            _Setting = settings.Value;
        }
        public async Task<List<Employee>> GetAllDataAsync(List<int> Ids)
        {
            throw new NotImplementedException("GetAllDataAsync with Ids");
        }

        public async Task<Employee?> GetDataAsync(string UserName)
        {
            throw new NotImplementedException("GetDataAsync with UserName");
        }


        public async Task<List<Employee>> GetAllDataAsync(int page)
        {
            var query = _DbContext.Employees
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

            return await data.AsNoTracking().ToListAsync();
        }

        public async Task<Employee?> GetDataAsync(int ID)
        {
            var query = _DbContext.Employees
                .Where(x => x.EmployeeID == ID)
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
                })
                .AsNoTracking();

            _Logger.LogInformation("SQL Query: {Query}", query.ToQueryString());

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
            throw new NotImplementedException("Changed Password");
        }

        public async Task<Employee?> CreateDataAsync(DTOEmployeesCRequest employee)
        {
            throw new NotImplementedException("Create Data Async");
        }

        public async Task<Employee?> UpdateDataAsync(DTOEmployeesURequest employee)
        {
           throw new NotImplementedException("Update Data Async");
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
           throw new NotImplementedException("Delete Data Async");
        }

    }   
}

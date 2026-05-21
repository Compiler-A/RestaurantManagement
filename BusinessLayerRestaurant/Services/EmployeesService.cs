using ContractsLayerRestaurant.DTORequest.Employees;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{
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

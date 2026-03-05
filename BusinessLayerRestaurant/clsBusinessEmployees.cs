using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public interface IReadableEmployeesBusiness
    {
        Task<DTOEmployees?> LoginEmployeeAsync(string UserName, string Password);
        Task<List<DTOEmployees>> GetAllEmployeesAsync(int page);
        Task<DTOEmployees?> GetEmployeeAsync(int ID);
        Task<DTOEmployees?> GetEmployeeAsync(string UserName);
    }

    public interface IWritableEmployeesBusiness
    {
        Task<bool> Save();
        Task<bool> Delete(int ID);
        Task<bool> ChangePasswordAsync(ChangedPassword Changed);
    }

    public interface IBusinessEmployees : IReadableEmployeesBusiness, IWritableEmployeesBusiness
    {
        DTOEmployees? Employees { get; set; }
    }

    public class clsBusinessEmployees : IBusinessEmployees
    {
        public enum enMode
        {
            Add, Update
        }
        public enMode Mode { get; set; } = enMode.Add;

        private DTOEmployees? _employees { get; set; }
        public DTOEmployees? Employees
        {
            get => _employees;
            set => _employees = value;
        }
        private IDataEmployee _dataEmployees;
        private IBusinessJobRoles _BusinessJobRoles;

        public clsBusinessEmployees(IDataEmployee employee, IBusinessJobRoles job)
        {
            _dataEmployees = employee;
            _BusinessJobRoles = job;
            Mode = enMode.Add;
        }

        public async Task<bool> ChangePasswordAsync(ChangedPassword Changed)
        {
            if (Changed == null || Changed.ID <= 0)
            {
                return false;
            }
            return await _dataEmployees.ChangedPassword(Changed);
        }
        public async Task<DTOEmployees?> GetEmployeeAsync(string UserName)
        {
            var dto = await _dataEmployees.GetEmployee(UserName);
            if (dto == null)
            {
                return null;
            }
            _employees = dto;
            _employees!.JobRoles = (await _BusinessJobRoles.GetJobRolesAsync(_employees.JobID))!;
            Mode = enMode.Update;
            return dto;
        }

        public async Task<DTOEmployees?> LoginEmployeeAsync(string UserName, string Password)
        {
            var dto = await _dataEmployees.LoginEmployee(UserName, Password);
            if (dto)
            {
                var d = await _dataEmployees.GetEmployee(UserName);
                d!.JobRoles = (await _BusinessJobRoles.GetJobRolesAsync(d.JobID))!;
                Mode = enMode.Update;
                return d;
            }
            return null;
        }
        public async Task<List<DTOEmployees>> GetAllEmployeesAsync(int page)
        {
            var list = await _dataEmployees.GetAllEmployees(page);

            foreach (var emp in list)
            {
                emp.JobRoles = (await _BusinessJobRoles.GetJobRolesAsync(emp.JobID))!;
            }

            return list;
        }


        public async Task<DTOEmployees?> GetEmployeeAsync(int ID)
        {
            var dto = await _dataEmployees.GetEmployee(ID);
            if (dto == null)
            {
                return null;
            }
            _employees = dto;
            _employees!.JobRoles = (await _BusinessJobRoles.GetJobRolesAsync(_employees.JobID))!;
            Mode = enMode.Update;
            return dto;
        }

        private async Task<bool> _Add()
        {
            if (_employees == null)
            {
                return false;
            }
            _employees.Password = clsGlobal.HashString(_employees.Password);
            _employees.ID = await _dataEmployees.Add(_employees); 
            if (_employees.ID > 0)
            {
                Mode = enMode.Update;
                return true;
            }
            return false;
        }

        private async Task<bool> _Update()
        {
            if (_employees == null || _employees.ID <= 0)
            {
                return false;
            }
            _employees.Password = clsGlobal.HashString(_employees.Password);
            return await _dataEmployees.Update(_employees);
        }

        public async Task<bool> Save()
        {
            bool result = false;
            switch(Mode)
            {
                case enMode.Add:
                    result = await _Add();
                    break;
                case enMode.Update:
                    result = await _Update();
                    break;
                default:
                    result = false;
                    break;
            }
            if (result)
            {
                _employees!.JobRoles = (await _BusinessJobRoles.GetJobRolesAsync(_employees.JobID))!;
            }
            
            return result;
        }

        public async Task<bool> Delete(int ID)
        {
            return await _dataEmployees.Delete(ID);
        }
    }
}

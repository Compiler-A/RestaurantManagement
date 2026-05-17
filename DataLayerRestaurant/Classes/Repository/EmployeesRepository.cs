using ContractsLayerRestaurant.DTORequest.Employees;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;


namespace DataLayerRestaurant.Classes.Repository
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private IEmployeesRepositoryReader _IRead;
        private IEmployeesRepositoryWriter _IWrite;

        public EmployeesRepository(IEmployeesRepositoryReader Read, IEmployeesRepositoryWriter Write)
        {
            _IRead = Read;
            _IWrite = Write;
        }


        public async Task<List<Employee>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<List<Employee>> GetAllDataAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }
        public async Task<Employee?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<Employee?> GetDataAsync(string Name)
        {
            return await _IRead.GetDataAsync(Name);
        }

        public async Task<bool> ChangedDataPasswordAsync(DTOEmployeesChangedPassword Request)
        {
            return await _IWrite.ChangedDataPasswordAsync(Request);
        }

        public async Task<Employee?> CreateDataAsync(DTOEmployeesCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }
        public async Task<Employee?> UpdateDataAsync(DTOEmployeesURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }
}

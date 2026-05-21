using ContractsLayerRestaurant.DTORequest.Tables;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{

    public class TablesService : ITablesService
    {
        private ITablesServiceContainer _Interfaces;
        private ITablesServiceWriter _IWrite;
        private ITablesServiceReader _IRead;

        public TablesService(ITablesServiceContainer table, ITablesServiceWriter write,
            ITablesServiceReader read)
        {
            _Interfaces = table;
            _IWrite = write;
            _IRead = read;
        }


        public IStatusTablesService IStatusTable
        {
            get => _Interfaces.IBusinessStatusTable;
            set => _Interfaces.IBusinessStatusTable = value;
        }

        public async Task<List<Table>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }
        public async Task<Table?> GetAsync(int id)
        {
            return await _IRead.GetAsync(id);
        }
        public async Task<List<Table>> GetAllAsync()
        {
            return await _IRead.GetAllAsync();
        }
        public async Task<List<Table>> GetFilter1Async(DTOTablesFilterStatusTableRequest Request)
        {
            return await _IRead.GetFilter1Async(Request);
        }
        public async Task<List<Table>> GetFilter2Async(DTOTablesFilterSeatTableRequest Request)
        {
            return await _IRead.GetFilter2Async(Request);
        }
        public async Task<Table?> GetByNameAsync(string tableNumber)
        {
            return await _IRead.GetByNameAsync(tableNumber);
        }
        public async Task<List<Table>> GetFilter3Async(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            return await _IRead.GetFilter3Async(Request);
        }
        public async Task<List<Table>> GetAllAvailablesAsync()
        {
            return await _IRead.GetAllAvailablesAsync();
        }
        public async Task<Table?> CreateAsync(DTOTablesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<Table?> UpdateAsync(DTOTablesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }
        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

using ContractsLayerRestaurant.DTORequest.Tables;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class TablesRepository : ITablesRepository
    {
        ITablesRepositoryWriter _IWrite;
        ITablesRepositoryReader _IRead;

        public TablesRepository(ITablesRepositoryWriter write, ITablesRepositoryReader read)
        {
            _IWrite = write;
            _IRead = read;
        }

        public async Task<List<Table>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<List<Table>> GetAllDataAvailablesAsync()
        {
            return await _IRead.GetAllDataAvailablesAsync();
        }
        public async Task<List<Table>> GetAllDataAsync()
        {
            return await _IRead.GetAllDataAsync();
        }
        public async Task<Table?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<Table?> GetDataByNameAsync(string TableNumber)
        {
            return await _IRead.GetDataByNameAsync(TableNumber);
        }

        public async Task<List<Table>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            return await _IRead.GetFilterStatusAndSeatDataAsync(Request);
        }
        public async Task<List<Table>> GetAllDataAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }

        public async Task<List<Table>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request)
        {
            return await _IRead.GetFilterStatusDataAsync(Request);
        }

        public async Task<List<Table>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request)
        {
            return await _IRead.GetFilterSeatDataAsync(Request);
        }

        public async Task<Table?> CreateDataAsync(DTOTablesCRequest Tables)
        {
            return await _IWrite.CreateDataAsync(Tables);
        }
        public async Task<Table?> UpdateDataAsync(DTOTablesURequest Table)
        {
            return await _IWrite.UpdateDataAsync(Table);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }

    }
}

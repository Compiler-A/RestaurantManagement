using ContractsLayerRestaurant.DTORequest.StatusTables;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class StatusTablesRepository : IStatusTablesRepository
    {
        private IStatusTablesRepositoryReader _IRead;
        private IStatusTablesRepositoryWriter _IWrite;
        public StatusTablesRepository(IStatusTablesRepositoryReader Read, IStatusTablesRepositoryWriter Write)
        {
            _IRead = Read;
            _IWrite = Write;
        }

        public async Task<List<StatusTable>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<bool> isFindDataAsync(int id)
        {
            return await _IRead.isFindDataAsync(id);

        }
        public async Task<StatusTable?> GetDataAsync(int id)
        {
            return await _IRead.GetDataAsync(id);
        }

        public async Task<List<StatusTable>> GetAllDataAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }
        public async Task<StatusTable?> CreateDataAsync(DTOStatusTablesCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }
        public async Task<StatusTable?> UpdateDataAsync(DTOStatusTablesURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteDataAsync(int id)
        {
            return await _IWrite.DeleteDataAsync(id);
        }
    }
}

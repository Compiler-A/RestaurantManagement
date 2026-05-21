using ContractsLayerRestaurant.DTORequest.StatusTables;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{

    public class StatusTablesService : IStatusTablesService
    {
        private IStatusTablesServiceReader _IRead;
        private IStatusTablesServiceWriter _IWrite;

        public StatusTablesService(IStatusTablesServiceReader read, IStatusTablesServiceWriter write)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<StatusTable>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<StatusTable?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }

        public async Task<bool> isFindAsync(int id)
        {
            return await _IRead.isFindAsync(id);
        }


        public async Task<StatusTable?> CreateAsync(DTOStatusTablesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<StatusTable?> UpdateAsync(DTOStatusTablesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

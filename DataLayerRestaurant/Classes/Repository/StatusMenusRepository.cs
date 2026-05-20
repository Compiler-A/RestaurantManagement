using ContractsLayerRestaurant.DTORequest.StatusMenus;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class StatusMenusRepository : IStatusMenusRepository
    {
        IStatusMenusRepositoryReader _IRead;
        IStatusMenusRepositoryWriter _IWrite;

        public StatusMenusRepository(IStatusMenusRepositoryReader read, IStatusMenusRepositoryWriter write)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<StatusMenu>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<List<StatusMenu>> GetAllDataAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }

        public async Task<StatusMenu?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<StatusMenu?> CreateDataAsync(DTOStatusMenusCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }

        public async Task<StatusMenu?> UpdateDataAsync(DTOStatusMenusURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }
}

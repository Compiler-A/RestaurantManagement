using ContractsLayerRestaurant.DTORequest.MenuItems;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class MenuItemsRepository : IMenuItemsRepository
    {

        IMenuItemsRepositoryReader _IRead;
        IMenuItemsRepositoryWriter _IWrite;

        public MenuItemsRepository(IMenuItemsRepositoryReader iRead, IMenuItemsRepositoryWriter iWrite)
        {
            _IRead = iRead;
            _IWrite = iWrite;
        }

        public async Task<List<MenuItem>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<List<MenuItem>> GetAllDataAvailablesAsync()
        {
            return await _IRead.GetAllDataAvailablesAsync();
        }
        public async Task<List<MenuItem>> GetAllDataAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }


        public async Task<List<MenuItem>> GetAllDataFiltersAsync(DTOMenuItemsFilterRequest Request)
        {
            return await _IRead.GetAllDataFiltersAsync(Request);
        }


        public async Task<MenuItem?> GetDataAsync(int id)
        {
            return await _IRead.GetDataAsync(id);
        }

        public async Task<MenuItem?> CreateDataAsync(DTOMenuItemsCRequest menuItem)
        {
            return await _IWrite.CreateDataAsync(menuItem);
        }

        public async Task<MenuItem?> UpdateDataAsync(DTOMenuItemsURequest menuItem)
        {
            return await _IWrite.UpdateDataAsync(menuItem);
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            return await _IWrite.DeleteDataAsync(id);
        }
    }
}

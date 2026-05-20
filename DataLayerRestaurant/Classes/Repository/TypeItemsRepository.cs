using ContractsLayerRestaurant.DTORequest.TypeItems;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes.Repository
{

    public class TypeItemsRepository : ITypeItemsRepository
    {
        ITypeItemsRepositoryReader _IRead;
        ITypeItemsRepositoryWriter _IWrite;

        public TypeItemsRepository(ITypeItemsRepositoryReader Read, ITypeItemsRepositoryWriter Write)
        {
            _IRead = Read;
            _IWrite = Write;
        }

        public async Task<List<TypeItem>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<TypeItem?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }
        public async Task<List<TypeItem>> GetAllDataAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }

        public async Task<TypeItem?> CreateDataAsync(DTOTypeItemsCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }
        public async Task<TypeItem?> UpdateDataAsync(DTOTypeItemsURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }

    }


}

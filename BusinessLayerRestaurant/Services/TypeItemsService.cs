using ContractsLayerRestaurant.DTORequest.TypeItems;
using ContractsLayerRestaurant.Interfaces.Services;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Services
{
    public class TypeItemsService : ITypeItemsService
    {
        ITypeItemsServiceWriter _IWrite;
        ITypeItemsServiceReader _IRead;

        public TypeItemsService(ITypeItemsServiceWriter write, ITypeItemsServiceReader read)
        {
            _IWrite = write;
            _IRead = read;
        }

        public async Task<TypeItem?> GetAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }
        public async Task<List<TypeItem>> GetAllAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<TypeItem?> CreateAsync(DTOTypeItemsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<TypeItem?> UpdateAsync(DTOTypeItemsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}

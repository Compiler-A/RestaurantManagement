using ContractsLayerRestaurant.DTORequest.TypeItems;
using DataLayerRestaurant.Data;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.EF
{
    public class TypeItemsRepositoryReaderEF : ITypeItemsRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public TypeItemsRepositoryReaderEF(IOptions<clsMySettings> Settings, AppDBContext Context)
        {
            _Settings = Settings.Value;
            _DbContext = Context;
        }

        public async Task<List<TypeItem>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.TypeItems
                .AsNoTracking()
                .Where(ti => Ids.Contains(ti.TypeItemID));
            
            return await query.ToListAsync();
        }

        public async Task<List<TypeItem>> GetAllDataAsync(int page)
        {
            var query = _DbContext.TypeItems
                .AsNoTracking()
                .OrderBy(ti => ti.TypeItemID);

            var data = query.Skip((page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }

        public async Task<TypeItem?> GetDataAsync(int id)
        {
            var query = _DbContext.TypeItems
                .AsNoTracking()
                .Where(ti => ti.TypeItemID == id);

            return await query.SingleOrDefaultAsync();
        }
    }

    public class TypeItemsRepositoryWriterEF : ITypeItemsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public TypeItemsRepositoryWriterEF(IOptions<clsMySettings> Settings , AppDBContext Context)
        {
            _Settings = Settings.Value;
            _DbContext = Context;
        }

        public async Task<TypeItem?> CreateDataAsync(DTOTypeItemsCRequest typeItem)
        {
            var Entity = new TypeItem
            {
                TypeName = typeItem.Name,
                TypeDescription = typeItem.Description
            };

            await _DbContext.TypeItems.AddAsync(Entity);
            await _DbContext.SaveChangesAsync();
            return Entity;
        }

        public async Task<TypeItem?> UpdateDataAsync(DTOTypeItemsURequest typeItem)
        {
            var existingEntity = await _DbContext.TypeItems.FindAsync(typeItem.ID);
            if (existingEntity == null)
            {
                return null;
            }
            existingEntity.TypeName = typeItem.Name;
            existingEntity.TypeDescription = typeItem.Description;

            await _DbContext.SaveChangesAsync();

            return existingEntity;
        }

        public async Task<bool> DeleteDataAsync(int typeItemID)
        {
            var existingEntity = await _DbContext.TypeItems.FindAsync(typeItemID);
            if (existingEntity == null)
            {
                return false;
            }
            _DbContext.TypeItems.Remove(existingEntity);
            await _DbContext.SaveChangesAsync();
            return true;
        }
    }
}

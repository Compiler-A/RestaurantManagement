
using ContractsLayerRestaurant.DTORequest.StatusMenus;
using DataLayerRestaurant.Data;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.EF
{
    public class StatusMenusRepositoryReaderEF : IStatusMenusRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public StatusMenusRepositoryReaderEF(IOptions<clsMySettings> settings, AppDBContext Context)
        {
            _Settings = settings.Value;
            _DbContext = Context;
        }

        public async Task<List<StatusMenu>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.StatusMenus
                .AsNoTracking()
                .Where(sm => Ids.Contains(sm.StatusMenuID));

            return await query.ToListAsync();
        }

        public async Task<StatusMenu?> GetDataAsync(int id)
        {
           var query = _DbContext.StatusMenus
                .AsNoTracking()
                .Where(sm => sm.StatusMenuID == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<StatusMenu>> GetAllDataAsync(int page)
        {
            var query = _DbContext.StatusMenus
                .AsNoTracking()
                .OrderBy(sm => sm.StatusMenuID);

            var data = query.Skip((page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }
    }

    public class StatusMenusRepositoryWriterEF : IStatusMenusRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public StatusMenusRepositoryWriterEF(IOptions<clsMySettings> settings, AppDBContext Context)
        {
            _Settings = settings.Value;
            _DbContext = Context;
        }

        public async Task<StatusMenu?> CreateDataAsync(DTOStatusMenusCRequest statusMenu)
        {
            var Entity = new StatusMenu
            {
                StatusMenuName = statusMenu.Name,
                Description = statusMenu.Description
            };

            await _DbContext.StatusMenus.AddAsync(Entity);
            await _DbContext.SaveChangesAsync();
            return Entity;
        }

        public async Task<StatusMenu?> UpdateDataAsync(DTOStatusMenusURequest statusMenu)
        {
            var Entity = await _DbContext.StatusMenus.FindAsync(statusMenu.ID);
            if (Entity == null)
            {
                return null;
            }
            Entity.StatusMenuName = statusMenu.Name;
            Entity.Description = statusMenu.Description;

            await _DbContext.SaveChangesAsync();
            return Entity;
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            var Entity = await _DbContext.StatusMenus.FindAsync(id);
            if (Entity == null)
            {
                return false;
            }

            _DbContext.StatusMenus.Remove(Entity);
            await _DbContext.SaveChangesAsync();
            return true;
        }
    }

}

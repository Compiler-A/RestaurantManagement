

using ContractsLayerRestaurant.DTORequest.Settings;
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
    public class SettingsRepositoryReaderEF : ISettingsRepositoryReader
    {

        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public SettingsRepositoryReaderEF(IOptions<clsMySettings> settings, AppDBContext Context)
        {
            _Settings = settings.Value;
            _DbContext = Context;
        }
        public async Task<List<Setting>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.Settings
                .AsNoTracking()
                .Where(x => Ids.Contains(x.SettingID));
            
            return await query.ToListAsync();
        }

        public async Task<List<Setting>> GetAllDataAsync(int page)
        {
            var query = _DbContext.Settings
                .AsNoTracking()
                .OrderBy(x => x.SettingID);

            var data = query.Skip((page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }

        public async Task<Setting?> GetDataAsync(int ID)
        {
            var query = _DbContext.Settings
                .AsNoTracking()
                .Where(x => x.SettingID == ID);

            return await query.FirstOrDefaultAsync();
        }
    }
    public class SettingsRepositoryWriterEF : ISettingsRepositoryWriter
    {

        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;
        public SettingsRepositoryWriterEF(IOptions<clsMySettings> settings, AppDBContext Context)
        {
            _Settings = settings.Value;
            _DbContext = Context;
        }

        public async Task<Setting?> CreateDataAsync(DTOSettingsCRequest dto)
        {
            var Entity = new Setting
            {
                Name = dto.Name,
                Value = dto.Value
            };

            await _DbContext.Settings.AddAsync(Entity);
            await _DbContext.SaveChangesAsync();

            return Entity;
        }
        public async Task<Setting?> UpdateDataAsync(DTOSettingsURequest DTO)
        {
            var Entity = await _DbContext.Settings.FindAsync(DTO.ID);
            if (Entity == null)
            {
                return null;
            }

            Entity.Name = DTO.Name;
            Entity.Value = DTO.Value;

            await _DbContext.SaveChangesAsync();
            return Entity;
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            var Entity = await _DbContext.Settings.FindAsync(ID);
            if (Entity == null)
            {
                return false;
            }

            _DbContext.Settings.Remove(Entity);
            await _DbContext.SaveChangesAsync();
            return true;
        }
    }
}



using ContractsLayerRestaurant.DTORequest.StatusTables;
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
    public class StatusTablesRepositoryReaderEF : IStatusTablesRepositoryReader
    {

        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public StatusTablesRepositoryReaderEF(IOptions<clsMySettings> Settings, AppDBContext DbContext)
        {
            _Settings = Settings.Value;
            _DbContext = DbContext;
        }

        public async Task<List<StatusTable>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.StatusTables
                .AsNoTracking()
                .Where(st => Ids.Contains(st.StatusTableID));

           return await query.ToListAsync();
        }
        public async Task<bool> isFindDataAsync(int id)
        {
            var query = _DbContext.StatusTables;

            return await query.AnyAsync(x => x.StatusTableID == id);
        }
        public async Task<StatusTable?> GetDataAsync(int id)
        {
            var query = _DbContext.StatusTables
                .AsNoTracking()
                .Where(x => x.StatusTableID == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<StatusTable>> GetAllDataAsync(int page)
        {
            var query = _DbContext.StatusTables
                .AsNoTracking()
                .OrderBy(x => x.StatusTableID);

            var data = query.Skip((page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }
    }


    public class StatusTablesRepositoryWriterEF : IStatusTablesRepositoryWriter
    {
        readonly private clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public StatusTablesRepositoryWriterEF(IOptions<clsMySettings> Settings, AppDBContext dBContext)
        {
            _Settings = Settings.Value;
            _DbContext = dBContext;
        }

        public async Task<StatusTable?> CreateDataAsync(DTOStatusTablesCRequest StatusTable)
        {
            var Entity = new StatusTable
            {
                StatusTableName = StatusTable.Name
            };
            await _DbContext.StatusTables.AddAsync(Entity);
            await _DbContext.SaveChangesAsync();
            return Entity;
        }


        public async Task<StatusTable?> UpdateDataAsync(DTOStatusTablesURequest StatusTable)
        {
            var Entity = await _DbContext.StatusTables.FindAsync(StatusTable.ID);
            if (Entity == null) 
            {
                return null;
            }

            Entity.StatusTableName = StatusTable.Name;
            await _DbContext.SaveChangesAsync();
            return Entity;
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            var Entity = await _DbContext.StatusTables.FindAsync(id);
            if (Entity == null)
            {
                return false;
            }

            _DbContext.StatusTables.Remove(Entity);
            await _DbContext.SaveChangesAsync();

            return true;
        }
    }
}

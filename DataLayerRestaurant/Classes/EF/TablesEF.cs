using ContractsLayerRestaurant.DTORequest.Tables;
using DataLayerRestaurant.Data;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.EF
{
    public class TablesRepositoryReaderEF : ITablesRepositoryReader
    {

        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public TablesRepositoryReaderEF(IOptions<clsMySettings> settings, AppDBContext DbContext)
        {
            _Settings = settings.Value;
            _DbContext = DbContext;
        }

        public async Task<List<Table>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Where(x => Ids.Contains(x.TableID))
                .Include(x => x.StatusTable);
        
            return await query.ToListAsync();
        }

        public async Task<List<Table>> GetAllDataAvailablesAsync()
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Where(x => x.StatusTableID == 1)
                .Include(x => x.StatusTable);

            return await query.ToListAsync();
        }


        public async Task<List<Table>> GetAllDataAsync()
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Include(x => x.StatusTable);

            return await query.ToListAsync();
        }
        public async Task<Table?> GetDataAsync(int ID)
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Where(x => x.TableID == ID)
                .Include(x => x.StatusTable);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Table?> GetDataByNameAsync(string TableNumber)
        {
            var q = _DbContext.Tables
                .AsNoTracking()
                .Where(x => x.TableNumber == TableNumber)
                .Include(x => x.StatusTable);

            return await q.FirstOrDefaultAsync();
        }

        public async Task<List<Table>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Where(x => (x.StatusTableID == Request.StatusTableID || x.StatusTableID == 0) 
                && (x.Seats ==  Request.Seats || x.Seats == 0))
                .Include(x => x.StatusTable);

            var data = query.Skip((Request.Page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }
        public async Task<List<Table>> GetAllDataAsync(int page)
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Include(x => x.StatusTable);

            var data = query.Skip((page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }

        public async Task<List<Table>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request)
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Where(x => x.StatusTableID == Request.StatusTableID)
                .Include(x => x.StatusTable);

            var data = query.Skip((Request.Page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }

        public async Task<List<Table>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request)
        {
            var query = _DbContext.Tables
                .AsNoTracking()
                .Where(x => x.StatusTableID == Request.Seats)
                .Include(x => x.StatusTable);

            var data = query.Skip((Request.Page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();

        }
    }

    public class TablesRepositoryWriterEF : ITablesRepositoryWriter
    {

        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;
        
        public TablesRepositoryWriterEF(IOptions<clsMySettings> settings, AppDBContext DbContext)
        {
            _Settings = settings.Value;
            _DbContext = DbContext;
        }

        public async Task<Table?> CreateDataAsync(DTOTablesCRequest Tables)
        {
            var Entity = new Table
            {
                Seats = Tables.Seats,
                StatusTableID = Tables.StatusTableID,
                TableNumber = "Temp"
            };
            await _DbContext.Tables.AddAsync(Entity);

            await _DbContext.SaveChangesAsync();
            Entity.TableNumber = $"T{Entity.TableID}-{Entity.Seats}";
            await _DbContext.SaveChangesAsync();

            var StatusTable = await _DbContext.StatusTables.FindAsync(Entity.StatusTableID);
            if (StatusTable == null)
            {
                return null;
            }
            Entity.StatusTable = StatusTable;
            return Entity;
        }

        public async Task<Table?> UpdateDataAsync(DTOTablesURequest Table)
        {
            var Entity = await _DbContext.Tables.FindAsync(Table.ID);

            if (Entity == null)
                return null;

            Entity.Seats = Table.Seats;
            Entity.StatusTableID = Table.StatusTableID;
            Entity.TableNumber = $"T{Table.ID}-{Table.Seats}";

            await _DbContext.SaveChangesAsync();

            var StatusTable = await _DbContext.StatusTables.FindAsync(Entity.StatusTableID);
            if (StatusTable == null)
            {
                return null;
            }
            Entity.StatusTable = StatusTable;

            return Entity;

        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            var Entity = await _DbContext.Tables.FindAsync(ID);

            if (Entity == null)
                return false;

            _DbContext.Tables.Remove(Entity);
            return await _DbContext.SaveChangesAsync() > 0;
        }
    }
}

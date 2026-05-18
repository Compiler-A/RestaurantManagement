

using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DataLayerRestaurant.Data;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.EF
{
    public class StatusOrdersRepositoryReaderEF : IStatusOrdersRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public StatusOrdersRepositoryReaderEF(IOptions<clsMySettings> Settings, AppDBContext DbContext)
        {
            _Settings = Settings.Value;
            _DbContext = DbContext;
        }

        public async Task<List<StatusOrder>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.StatusOrders
                .AsNoTracking()
                .Where(so => Ids.Contains(so.StatusOrderID));

            return await query.ToListAsync();
        }


        public async Task<StatusOrder?> GetDataAsync(int ID)
        {
            var query = _DbContext.StatusOrders
                .AsNoTracking()
                .Where(so => so.StatusOrderID == ID);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<StatusOrder>> GetAllDataAsync(int Page)
        {
            var query = _DbContext.StatusOrders
                .AsNoTracking()
                .OrderBy(x => x.StatusOrderID);

            var data = query.Skip((Page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }
    }

    public class StatusOrdersRepositoryWriterEF : IStatusOrdersRepositoryWriter
    {

        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public StatusOrdersRepositoryWriterEF(IOptions<clsMySettings> Settings, AppDBContext DbContext)
        {
            _Settings = Settings.Value;
            _DbContext = DbContext;
        }

        public async Task<StatusOrder?> CreateDataAsync(DTOStatusOrdersCRequest Request)
        {
            var Entity = new StatusOrder
            {
                StatusOrderName = Request.Name
            };

            await _DbContext.StatusOrders.AddAsync(Entity);
            await _DbContext.SaveChangesAsync();

            return Entity;
        }
        public async Task<StatusOrder?> UpdateDataAsync(DTOStatusOrdersURequest Request)
        {
            var Entity = await _DbContext.StatusOrders.FindAsync(Request.ID);
            if (Entity == null)
                return null;

            Entity.StatusOrderName = Request.Name;
            await _DbContext.SaveChangesAsync();
            return Entity;
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            var Entity = await _DbContext.StatusOrders.FindAsync(ID);
            if (Entity == null)
                return false;

            _DbContext.StatusOrders.Remove(Entity);
            await _DbContext.SaveChangesAsync();
            return true;
        }
    }
}

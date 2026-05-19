using ContractsLayerRestaurant.DTORequest.OrderDetails;
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
    public class OrderDetailsRepositoryReaderEF : IOrderDetailsRepositoryReader
    {

        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;

        public OrderDetailsRepositoryReaderEF(IOptions<clsMySettings> Settings, AppDBContext DbContext)
        {
            _Settings = Settings.Value;
            _DbContext = DbContext;
        }
        public async Task<List<OrderDetail>> GetAllDataByOrderIdsAsync(List<int> Ids)
        {
            var query = _DbContext.OrderDetails
                .AsNoTracking()
                .Where(x => Ids.Contains(x.OrderID))
                .Select(x => new OrderDetail
                {
                    OrderDetailID = x.OrderDetailID,
                    OrderID = x.OrderID,
                    Item = new MenuItem
                    {
                        ItemID = x.ItemID,
                        ItemName = x.Item.ItemName
                    },
                    Quantity = x.Quantity,
                    SubTotal = x.SubTotal
                });

            return await query.ToListAsync();
        }
        public async Task<List<OrderDetail>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.OrderDetails
                .AsNoTracking()
                .Where(x => Ids.Contains(x.OrderDetailID))
                .Select(x => new OrderDetail
                {
                    OrderDetailID = x.OrderDetailID,
                    OrderID = x.OrderID,
                    Item = new MenuItem
                    {
                        ItemID = x.ItemID,
                        ItemName = x.Item.ItemName
                    },
                    Quantity = x.Quantity,
                    SubTotal = x.SubTotal
                });

            return await query.ToListAsync();
        }

        public async Task<OrderDetail?> GetDataAsync(int ID)
        {
            var query = _DbContext.OrderDetails
                .AsNoTracking()
                .Where(x => x.OrderDetailID == ID)
                .Select(x => new OrderDetail
                {
                    OrderDetailID = x.OrderDetailID,
                    OrderID = x.OrderID,
                    Item = new MenuItem
                    {
                        ItemID = x.ItemID,
                        ItemName = x.Item.ItemName
                    },
                    Quantity = x.Quantity,
                    SubTotal = x.SubTotal
                });

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<OrderDetail>> GetAllDataAsync(int page)
        {
            var query = _DbContext.OrderDetails
                 .AsNoTracking()
                 .Select(x => new OrderDetail
                 {
                     OrderDetailID = x.OrderDetailID,
                     OrderID = x.OrderID,
                     Item = new MenuItem
                     {
                         ItemID = x.ItemID,
                         ItemName = x.Item.ItemName
                     },
                     Quantity = x.Quantity,
                     SubTotal = x.SubTotal
                 });
            var data = query.Skip((page - 1) * _Settings.RowsPerPage)
                            .Take(_Settings.RowsPerPage);

            return await data.ToListAsync();
        }

        public async Task<List<OrderDetail>> GetAllDataByOrderIDAsync(int orderID)
        {
            var query = _DbContext.OrderDetails
                .AsNoTracking()
                .Where(x => x.OrderID == orderID)
                .Select(x => new OrderDetail
                {
                    OrderDetailID = x.OrderDetailID,
                    OrderID = x.OrderID,
                    Item = new MenuItem
                    {
                        ItemID = x.ItemID,
                        ItemName = x.Item.ItemName
                    },
                    Quantity = x.Quantity,
                    SubTotal = x.SubTotal
                });

            return await query.ToListAsync();
        }
    }

    public class OrderDetailsRepositoryWriterEF : IOrderDetailsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        private readonly AppDBContext _DbContext;
        public OrderDetailsRepositoryWriterEF(IOptions<clsMySettings> settings , AppDBContext dbContext)
        {
            _Settings = settings.Value;
            _DbContext = dbContext;
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            var Entity = await _DbContext.OrderDetails.FindAsync(id);
            if (Entity == null)
            {
                return false;
            }
            _DbContext.OrderDetails.Remove(Entity);
            await _DbContext.SaveChangesAsync();
            return await _DbContext.SaveChangesAsync() > 0;
        }

        public async Task<OrderDetail?> CreateDataAsync(DTOOrderDetailsCRequest dto)
        {
            var menuItem = await _DbContext.MenuItems.FindAsync(dto.ItemID);

            if (menuItem == null)
            {
                return null;
            }

            var order = await _DbContext.Orders.FindAsync(dto.OrderID);

            if (order == null)
            {
                return null;
            }

            var entity = new OrderDetail
            {
                OrderID = dto.OrderID,
                ItemID = dto.ItemID,
                Quantity = dto.Quantity,
                SubTotal = dto.Quantity * menuItem.Price
            };

            await _DbContext.OrderDetails.AddAsync(entity);

            order.TotalAmount += entity.SubTotal;

            await _DbContext.SaveChangesAsync();

            entity.Item = menuItem;

            return entity;
        }

        public async Task<OrderDetail?> UpdateDataAsync(DTOOrderDetailsURequest dto)
        {
            await using var transaction = await _DbContext.Database.BeginTransactionAsync();

            try
            {
                var entity = await _DbContext.OrderDetails
                    .FirstOrDefaultAsync(x => x.OrderDetailID == dto.ID);

                if (entity == null)
                    return null;

                var menuItem = await _DbContext.MenuItems
                    .FirstOrDefaultAsync(x => x.ItemID == dto.ItemID);

                if (menuItem == null)
                    return null;

                int oldOrderId = entity.OrderID;

                entity.ItemID = dto.ItemID;
                entity.OrderID = dto.OrderID;
                entity.Quantity = dto.Quantity;
                entity.SubTotal = dto.Quantity * menuItem.Price;

                await _DbContext.SaveChangesAsync();

                var affectedOrders = new HashSet<int>
                {
                    oldOrderId,
                    dto.OrderID
                };

                foreach (var orderId in affectedOrders)
                {
                    var order = await _DbContext.Orders
                        .FirstOrDefaultAsync(x => x.OrderID == orderId);

                    if (order != null)
                    {
                        order.TotalAmount = await _DbContext.OrderDetails
                            .Where(x => x.OrderID == orderId)
                            .SumAsync(x => (decimal?)x.SubTotal) ?? 0;
                    }
                }

                await _DbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                entity.Item = menuItem;

                return entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

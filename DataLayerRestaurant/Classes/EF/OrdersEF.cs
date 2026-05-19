using ContractsLayerRestaurant.DTORequest.Orders;
using DataLayerRestaurant.Data;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DataLayerRestaurant.Classes.EF
{
    public class OrderDetailBatchLoader : IOrderDetailBatchLoader
    {
        private readonly IOrderDetailsRepositoryReader _OrderDetail;
        public OrderDetailBatchLoader(IOrderDetailsRepositoryReader orderDetail)
        {
            _OrderDetail = orderDetail;
        }
        public async Task LoadBatchAsync(List<Order> orders)
        {
            var orderIds = orders.Select(o => o.OrderID).ToList();
            var details = await _OrderDetail.GetAllDataByOrderIdsAsync(orderIds);
            var detailsByOrderId = details.GroupBy(d => d.Order!.OrderID).ToDictionary(g => g.Key, g => g.ToList());
            foreach (var order in orders)
            {
                if (detailsByOrderId.TryGetValue(order.OrderID, out var orderDetails))
                {
                    order.OrderDetails = orderDetails;
                }
                else
                {
                    order.OrderDetails = new List<OrderDetail>();
                }
            }
        }
    }

    public class OrdersRepositoryReaderEF : IOrdersRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private readonly IOrderDetailBatchLoader _BatchLoader;
        private readonly AppDBContext _DbContext;

        public OrdersRepositoryReaderEF(IOptions<clsMySettings> settings, IOrderDetailBatchLoader batchLoader, AppDBContext dbContext)
        {
            _Settings = settings.Value;
            _BatchLoader = batchLoader;
            _DbContext = dbContext;
        }

        public async Task<List<Order>> GetAllDataAsync(List<int> Ids)
        {
            var query = _DbContext.Orders
                .AsNoTracking()
                .Where(x => Ids.Contains(x.OrderID))
                .Select(x => new Order
                {
                    OrderID = x.OrderID,
                    TableID = x.TableID,
                    EmployeeID = x.EmployeeID,
                    StatusOrderID = x.StatusOrderID,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    Employee = new Employee
                    {
                        EmployeeID = x.Employee.EmployeeID,
                        Username = x.Employee.Username
                    },
                    StatusOrder = new StatusOrder
                    {
                        StatusOrderID = x.StatusOrder.StatusOrderID,
                        StatusOrderName = x.StatusOrder.StatusOrderName
                    },
                    Table = new Table
                    {
                        TableID = x.Table.TableID,
                        TableNumber = x.Table.TableNumber
                    }
                });

            var data = await query
                .ToListAsync();
            await _BatchLoader.LoadBatchAsync(data);
            return data;
        }

        public async Task<List<Order>> GetAllDataAsync(int page)
        {
            var query = _DbContext.Orders
                .AsNoTracking()
                .Select(x => new Order
                {
                    OrderID = x.OrderID,
                    TableID = x.TableID,
                    EmployeeID = x.EmployeeID,
                    StatusOrderID = x.StatusOrderID,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    Employee = new Employee
                    {
                        EmployeeID = x.Employee.EmployeeID,
                        Username = x.Employee.Username
                    },
                    StatusOrder = new StatusOrder
                    {
                        StatusOrderID = x.StatusOrder.StatusOrderID,
                        StatusOrderName = x.StatusOrder.StatusOrderName
                    },
                    Table = new Table
                    {
                        TableID = x.Table.TableID,
                        TableNumber = x.Table.TableNumber
                    }
                });

            var data = await query
                .Skip((page - 1) * _Settings.RowsPerPage)
                .Take(_Settings.RowsPerPage)
                .ToListAsync();
            await _BatchLoader.LoadBatchAsync(data);
            return data;
        }

        public async Task<Order?> GetDataAsync(int ID)
        {
            var query = _DbContext.Orders
                .AsNoTracking()
                .Where(x => x.OrderID == ID)
                .Select(x => new Order
                {
                    OrderID = x.OrderID,
                    TableID = x.TableID,
                    EmployeeID = x.EmployeeID,
                    StatusOrderID = x.StatusOrderID,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    Employee = new Employee
                    {
                        EmployeeID = x.Employee.EmployeeID,
                        Username = x.Employee.Username
                    },
                    StatusOrder = new StatusOrder
                    {
                        StatusOrderID = x.StatusOrder.StatusOrderID,
                        StatusOrderName = x.StatusOrder.StatusOrderName
                    },
                    Table = new Table
                    {
                        TableID = x.Table.TableID,
                        TableNumber = x.Table.TableNumber
                    }
                });
            
            var data = await query.FirstOrDefaultAsync();
            if (data == null)
            {
                return null;
            }
            await _BatchLoader.LoadBatchAsync(new List<Order> { data });
            return data;
        }

        public async Task<List<Order>?> GetFilterDataAsync(DTOOrderFilterRequest Request)
        {
            var query = _DbContext.Orders
                .AsNoTracking()
                .Where(x => (x.StatusOrderID == Request.StatusOrderID || x.StatusOrderID == 0) 
                && (x.TableID == Request.TableID || Request.TableID == 0) 
                && (x.EmployeeID == Request.EmployeeID || Request.EmployeeID == 0))
                .OrderBy(x => x.OrderID);

            var SelectQuery = query
                .Select(x => new Order
                {
                    OrderID = x.OrderID,
                    TableID = x.TableID,
                    EmployeeID = x.EmployeeID,
                    StatusOrderID = x.StatusOrderID,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    Employee = new Employee
                    {
                        EmployeeID = x.Employee.EmployeeID,
                        Username = x.Employee.Username
                    },
                    StatusOrder = new StatusOrder
                    {
                        StatusOrderID = x.StatusOrder.StatusOrderID,
                        StatusOrderName = x.StatusOrder.StatusOrderName
                    },
                    Table = new Table
                    {
                        TableID = x.Table.TableID,
                        TableNumber = x.Table.TableNumber
                    }
                });

            var data = await SelectQuery
                .Skip((Request.Page - 1) * _Settings.RowsPerPage)
                .Take(_Settings.RowsPerPage)
                .ToListAsync();

            await _BatchLoader.LoadBatchAsync(data);
            return data;
        }
    }

    public class OrdersRepositoryWriterEF : IOrdersRepositoryWriter
    {

        private readonly clsMySettings _Settings;
        private readonly IOrderDetailBatchLoader _BatchLoader;
        private readonly AppDBContext _DbContext;
        public OrdersRepositoryWriterEF(IOptions<clsMySettings> settings, IOrderDetailBatchLoader BatchLoader, AppDBContext DbContext)
        {
            _Settings = settings.Value;
            _BatchLoader = BatchLoader;
            _DbContext = DbContext;
        }

        public async Task<Order?> CreateDataAsync(DTOOrderCRequest dto)
        {
            await using var transaction = await _DbContext.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    TableID = dto.TableID,
                    EmployeeID = dto.EmployeeID,
                    OrderDate = dto.OrderDate,
                    StatusOrderID = dto.StatusOrderID,
                    TotalAmount = dto.TotalAmount
                };

                await _DbContext.Orders.AddAsync(order);
                await _DbContext.SaveChangesAsync(); // generates OrderID

                var table = await _DbContext.Tables
                    .FirstOrDefaultAsync(x => x.TableID == dto.TableID);

                if (table != null)
                {
                    table.StatusTableID =
                        (dto.StatusOrderID is 4 or 5) ? 1 : 2;
                }

                await _DbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                // return full order like SP output
                var data = await _DbContext.Orders
                    .AsNoTracking()
                    .Include(x => x.Table)
                    .Include(x => x.Employee)
                    .Include(x => x.StatusOrder)
                    .FirstOrDefaultAsync(x => x.OrderID == order.OrderID);
                if (data == null)
                {
                    return null;
                }
                await _BatchLoader.LoadBatchAsync(new List<Order> { data });
                return data;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order?> UpdateDataAsync(DTOOrderURequest dto)
        {
            await using var transaction = await _DbContext.Database.BeginTransactionAsync();

            try
            {
                var order = await _DbContext.Orders
                    .FirstOrDefaultAsync(x => x.OrderID == dto.OrderID);

                if (order == null)
                    return null;

                order.TableID = dto.TableID;
                order.EmployeeID = dto.EmployeeID;
                order.OrderDate = dto.OrderDate;
                order.StatusOrderID = dto.StatusOrderID;
                order.TotalAmount = dto.TotalAmount;

                var table = await _DbContext.Tables
                    .FirstOrDefaultAsync(x => x.TableID == dto.TableID);

                if (table != null)
                {
                    table.StatusTableID =
                        (dto.StatusOrderID is 4 or 5) ? 1 : 2;
                }

                await _DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                // reload with navigation (clean way)
                var data = await _DbContext.Orders
                    .AsNoTracking()
                    .Include(x => x.Table)
                    .Include(x => x.Employee)
                    .Include(x => x.StatusOrder)
                    .FirstOrDefaultAsync(x => x.OrderID == dto.OrderID);
                if (data == null)
                {
                    return null;
                }
                await _BatchLoader.LoadBatchAsync(new List<Order> { data });
                return data;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            var Entity = await _DbContext.Orders.FindAsync(ID);
            if (Entity == null)
            {
                return false;
            }

            _DbContext.Orders.Remove(Entity);
            

            return await _DbContext.SaveChangesAsync() > 0;
        }
    }
}

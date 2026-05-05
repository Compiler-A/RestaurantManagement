using ContractsLayerRestaurant.DTORequest.Orders;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes
{

    public class clsOrderDetailBatchLoader : IOrderDetailBatchLoader
    {
        private readonly IOrderDetailsRepositoryReader _OrderDetail;
        public clsOrderDetailBatchLoader(IOrderDetailsRepositoryReader orderDetail)
        {
            _OrderDetail = orderDetail;
        }
        public async Task LoadBatchAsync(List<Order> orders)
        {
            var orderIds = orders.Select(o => o.ID).ToList();
            var details = await _OrderDetail.GetAllDataByOrderIdsAsync(orderIds);
            var detailsByOrderId = details.GroupBy(d => d.Order!.ID).ToDictionary(g => g.Key, g => g.ToList());
            foreach (var order in orders)
            {
                if (detailsByOrderId.TryGetValue(order.ID, out var orderDetails))
                {
                    order.Details = orderDetails;
                }
                else
                {
                    order.Details = new List<OrderDetail>();
                }
            }
        }
    }

    public class clsOrdersRepositoryReader : IOrdersRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private readonly IOrderDetailBatchLoader _BatchLoader;

        public clsOrdersRepositoryReader(IOptions<clsMySettings> settings, IOrderDetailBatchLoader batchLoader)
        {
            _Settings = settings.Value;
            _BatchLoader = batchLoader;
        }

        public async Task<List<Order>> GetAllDataAsync(List<int> Ids)
        {
            List<Order> result = new List<Order>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_GetAllOrdersByIds", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;

                    var param = new SqlParameter("@Ids", SqlDbType.Structured)
                    {
                        TypeName = "dbo.IntList",
                        Value = CreateSqlRecords.CreateSqlRecord(Ids)
                    };
                    Command.Parameters.Add(param);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(OrderMapper.ReaderToEntityResult(reader));
                        }
                    }
                }
            }
            await _BatchLoader.LoadBatchAsync(result);
            return result;
        }

        public async Task<List<Order>> GetAllDataAsync(int page)
        {
            List<Order> result = new List<Order>();

            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_GetAllOrders", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            result.Add(OrderMapper.ReaderToEntityResult(Reader));
                        }
                    }
                }
            }
            await _BatchLoader.LoadBatchAsync(result);
            return result;
        }

        public async Task<Order?> GetDataAsync(int ID)
        {
            Order? result = null;

            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_GetOrderByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            result = OrderMapper.ReaderToEntityResult(Reader);
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
            await _BatchLoader.LoadBatchAsync(new List<Order> { result});
            return result;
        }

        public async Task<List<Order>?> GetFilterDataAsync(DTOOrderFilterRequest Request)
        {
            List<Order>? result = new List<Order>();

            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_GetFilterOrders", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", Request.Page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);
                    Command.Parameters.AddWithValue("@TableID", Request.TableID);
                    Command.Parameters.AddWithValue("@EmployeeID", Request.EmployeeID);
                    Command.Parameters.AddWithValue("@StatusOrderID", Request.StatusOrderID);


                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            result.Add(OrderMapper.ReaderToEntityResult(Reader));
                        }
                    }
                }
            }

            await _BatchLoader.LoadBatchAsync( result);
            return result;
        }
    }

    public class clsOrdersRepositoryWriter : IOrdersRepositoryWriter
    {

        private readonly clsMySettings _Settings;
        private readonly IOrderDetailBatchLoader _BatchLoader;
        public clsOrdersRepositoryWriter(IOptions<clsMySettings> settings, IOrderDetailBatchLoader BatchLoader)
        {
            _Settings = settings.Value;
            _BatchLoader = BatchLoader;
        }

        public async Task<Order?> CreateDataAsync(DTOOrderCRequest order)
        {
            Order? result = null;

            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_AddOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TableID", order.TableID);
                    Command.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
                    Command.Parameters.AddWithValue("@Date", order.OrderDate);
                    Command.Parameters.AddWithValue("@StatusID", order.StatusOrderID);
                    Command.Parameters.AddWithValue("@Amount", (object?)order.TotalAmount ?? DBNull.Value);


                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            result = OrderMapper.ReaderToEntityResult(Reader);
                        }
                    }

                }
            }
            if (result == null)
            {
                return null;
            }
            await _BatchLoader.LoadBatchAsync(new List<Order> { result });
            return result;

        }

        public async Task<Order?> UpdateDataAsync(DTOOrderURequest order)
        {
            Order? result = null;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_UpdateOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TableID", order.TableID);
                    Command.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
                    Command.Parameters.AddWithValue("@Date", order.OrderDate);
                    Command.Parameters.AddWithValue("@StatusID", order.StatusOrderID);
                    Command.Parameters.AddWithValue("@Amount", (object?)order.TotalAmount ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@ID", order.OrderID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            result = OrderMapper.ReaderToEntityResult(Reader);
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
            await _BatchLoader.LoadBatchAsync(new List<Order> { result });
            return result;

        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            bool Delete = false;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_DeleteOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    Delete = await Command.ExecuteNonQueryAsync() > 0;
                }

            }
            return Delete;
        }
    }

    public class clsOrdersRepository : IOrdersRepository
    {
        IOrdersRepositoryWriter _Write;
        IOrdersRepositoryReader _Read;

        public clsOrdersRepository(IOrdersRepositoryWriter write, IOrdersRepositoryReader read)
        {
            _Write = write;
            _Read = read;
        }

        public async Task<List<Order>> GetAllDataAsync(List<int> Ids)
        {
            return await _Read.GetAllDataAsync(Ids);
        }

        public async Task<List<Order>> GetAllDataAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }
        public async Task<Order?> GetDataAsync(int ID)
        {
            return await _Read.GetDataAsync(ID);
        }
        public async Task<List<Order>?> GetFilterDataAsync(DTOOrderFilterRequest Request)
        {
            return await _Read.GetFilterDataAsync(Request);
        }

        public async Task<Order?> CreateDataAsync(DTOOrderCRequest order)
        {
            return await _Write.CreateDataAsync(order);
        }
        public async Task<Order?> UpdateDataAsync(DTOOrderURequest order)
        {
            return await _Write.UpdateDataAsync(order);
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _Write.DeleteDataAsync(ID);
        }
    }
}
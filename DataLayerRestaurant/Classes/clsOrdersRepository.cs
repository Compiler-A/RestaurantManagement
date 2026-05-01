using ContractsLayerRestaurant.DTORequest.Orders;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System.Data;

namespace DataLayerRestaurant.Classes
{
    
    public class clsOrdersRepositoryComposition : ICompositionDataBase<Order> 
    {
        public Order GetDataFromDataBase(SqlDataReader reader)
        {
            return new Order
            {
                ID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                TableID = reader.GetInt32(reader.GetOrdinal("TableID")),
                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                StatusOrderID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
            };
        }
    }

    public class clsEmployeeBatchLoader : IRepositoryBatchsLoader<Order>
    {
        private readonly IEmployeesRepositoryReader _service;
        public clsEmployeeBatchLoader(IEmployeesRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<Order> orders)
        {
            var employeeIDs = orders.Select(e => e.EmployeeID).Distinct().ToList();
            if (!employeeIDs.Any())
                return;
            var roles = await _service.GetAllDataAsync(employeeIDs);
            var dict = roles.ToDictionary(r => r.ID);
            foreach (var emp in orders)
            {
                if (dict.TryGetValue(emp.EmployeeID, out var role))
                {
                    emp.employees = role;
                }
            }
        }
    }

    public class clsTableBatchLoader : IRepositoryBatchsLoader<Order>
    {
        private readonly ITablesRepositoryReader _service;

        public clsTableBatchLoader(ITablesRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<Order> orders)
        {
            var tableIDs = orders.Select(e => e.TableID).Distinct().ToList();

            if (!tableIDs.Any())
                return;

            var roles = await _service.GetAllDataAsync(tableIDs);

            var dict = roles.ToDictionary(r => r.ID);

            foreach (var emp in orders)
            {
                if (dict.TryGetValue(emp.TableID, out var role))
                {
                    emp.tables = role;
                }
            }
        }
    }
    public class clsStatusOrderBatchLoader : IRepositoryBatchsLoader<Order>
    {
        private readonly IStatusOrdersRepositoryReader _service;

        public clsStatusOrderBatchLoader(IStatusOrdersRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<Order> orders)
        {
            var StatusOrderID = orders.Select(e => e.StatusOrderID).Distinct().ToList();

            if (!StatusOrderID.Any())
                return;

            var roles = await _service.GetAllDataAsync(StatusOrderID);

            var dict = roles.ToDictionary(r => r.ID);

            foreach (var emp in orders)
            {
                if (dict.TryGetValue(emp.StatusOrderID, out var role))
                {
                    emp.statusOrders = role;
                }
            }
        }
    }


    public class clsOrdersRepositoryLoader : IOrdersRepositoryLoader
    {
        private IEnumerable<IRepositoryBatchsLoader<Order>> _Loaders;
        public clsOrdersRepositoryLoader(IEnumerable<IRepositoryBatchsLoader<Order>> Loader)
        {
            _Loaders = Loader;
        }
        public async Task LoadDataAsync(List<Order> item)
        {
            foreach (var item1 in _Loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsOrdersRepositoryReader : clsOrdersRepositoryComposition, IOrdersRepositoryReader
    {
        private readonly clsMySettings _Settings;
        private IOrdersRepositoryLoader _Loader;
        public clsOrdersRepositoryReader(IOptions<clsMySettings> settings, IOrdersRepositoryLoader loader)
        {
            _Settings = settings.Value;
            _Loader = loader;
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
                            result.Add(GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            await _Loader.LoadDataAsync(result);
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
                            result.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            await _Loader.LoadDataAsync(result);
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
                            result = GetDataFromDataBase(Reader);
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
            await _Loader.LoadDataAsync(new List<Order> { result });
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
                            result.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
            await _Loader.LoadDataAsync(result);
            return result;
        }
    }

    public class clsOrdersRepositoryWriter :clsOrdersRepositoryComposition, IOrdersRepositoryWriter
    {

        private readonly clsMySettings _Settings;
        private IOrdersRepositoryLoader _Loader;
        public clsOrdersRepositoryWriter(IOptions<clsMySettings> settings, IOrdersRepositoryLoader loader)
        {
            _Settings = settings.Value;
            _Loader = loader;
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
                            result = GetDataFromDataBase(Reader);
                        }
                    }

                }
            }
            if (result == null)
            {
                return null;
            }
            await _Loader.LoadDataAsync(new List<Order> { result });
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
                            result = GetDataFromDataBase(Reader);
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
            await _Loader.LoadDataAsync(new List<Order> { result });
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
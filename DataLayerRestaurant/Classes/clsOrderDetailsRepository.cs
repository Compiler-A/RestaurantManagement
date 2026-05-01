using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System.Data;

namespace DataLayerRestaurant.Classes
{
    public class clsOrderDetailsRepositoryComposition : ICompositionDataBase<OrderDetail>
    {
        public OrderDetail GetDataFromDataBase(SqlDataReader reader)
        {
            return new OrderDetail
            {
                ID = reader.GetInt32(reader.GetOrdinal("OrderDetailID")),
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                SubTotal = reader.GetDecimal(reader.GetOrdinal("SubTotal"))
            };
        }
    }




    public class clsMenuItemBatchLoader : IRepositoryBatchsLoader<OrderDetail>
    {
        private readonly IMenuItemsRepositoryReader _service;

        public clsMenuItemBatchLoader(IMenuItemsRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<OrderDetail> orders)
        {
            var menuItemIDs = orders.Select(e => e.ItemID).Distinct().ToList();

            if (!menuItemIDs.Any())
                return;

            var roles = await _service.GetAllDataAsync(menuItemIDs);

            var dict = roles.ToDictionary(r => r.ID);

            foreach (var emp in orders)
            {
                if (dict.TryGetValue(emp.ItemID, out var role))
                {
                    emp.Item = role;
                }
            }
        }
    }
    public class clsOrderBatchLoader : IRepositoryBatchsLoader<OrderDetail>
    {
        private readonly IOrdersRepositoryReader _service;

        public clsOrderBatchLoader(IOrdersRepositoryReader service)
        {
            _service = service;
        }

        public async Task LoadDataAsync(List<OrderDetail> orders)
        {
            var OrderIDs = orders.Select(e => e.OrderID).Distinct().ToList();

            if (!OrderIDs.Any())
                return;

            var roles = await _service.GetAllDataAsync(OrderIDs);

            var dict = roles.ToDictionary(r => r.ID);

            foreach (var emp in orders)
            {
                if (dict.TryGetValue(emp.OrderID, out var role))
                {
                    emp.Order = role;
                }
            }
        }
    }


    public class clsOrderDetailsRepositoryLoader : IOrderDetailsRepositoryLoader
    {
        private IEnumerable<IRepositoryBatchsLoader<OrderDetail>> _Loaders;
        public clsOrderDetailsRepositoryLoader(IEnumerable<IRepositoryBatchsLoader<OrderDetail>> Loader)
        {
            _Loaders = Loader;
        }
        public async Task LoadDataAsync(List<OrderDetail> item)
        {
            foreach (var item1 in _Loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsOrderDetailsRepositoryReader : clsOrderDetailsRepositoryComposition ,IOrderDetailsRepositoryReader
    {

        private readonly clsMySettings _Settings;
        private IOrderDetailsRepositoryLoader _Loader;
        public clsOrderDetailsRepositoryReader(IOptions<clsMySettings> Settings, IOrderDetailsRepositoryLoader Loader)
        {
            _Settings = Settings.Value;
            _Loader = Loader;
        }

        public async Task<List<OrderDetail>> GetAllDataAsync(List<int> Ids)
        {
            List<OrderDetail> result = new List<OrderDetail>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_GetAllOrderDetailsByIds", Connection))
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

        public async Task<OrderDetail?> GetDataAsync(int ID)
        {
            OrderDetail? result = null;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_GetOrderDetailByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            result = (GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
            await _Loader.LoadDataAsync(new List<OrderDetail> { result });
            return result;
        }
        public async Task<List<OrderDetail>> GetAllDataAsync(int page)
        {
            List<OrderDetail> result = new List<OrderDetail>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_GetAllOrderDetails", Connection))
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
        public async Task<List<OrderDetail>> GetAllDataByOrderIDAsync(int orderID)
        {
            List<OrderDetail> result = new List<OrderDetail>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_GetItemByOrderID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@OrderID", orderID);
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
    }

    public class clsOrderDetailsRepositoryWriter : clsOrderDetailsRepositoryComposition,IOrderDetailsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        private IOrderDetailsRepositoryLoader _Loader;
        public clsOrderDetailsRepositoryWriter(IOptions<clsMySettings> settings, IOrderDetailsRepositoryLoader loader)
        {
            _Settings = settings.Value; 
            _Loader = loader;
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            bool Delete = false;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_DeleteOrderDetail", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", id);

                    await Connection.OpenAsync();
                    Delete = await Command.ExecuteNonQueryAsync() > 0;
                }

                return Delete;
            }
        }
        public async Task<OrderDetail?> CreateDataAsync(DTOOrderDetailsCRequest dto)
        {
            OrderDetail? result = null;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_AddOrderDetail", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ItemID", dto.ItemID);
                    Command.Parameters.AddWithValue("@OrderID", dto.OrderID);
                    Command.Parameters.AddWithValue("@Quantity", dto.Quantity);

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

            await _Loader.LoadDataAsync(new List<OrderDetail> { result });
            return result;
        }

        public async Task<OrderDetail?> UpdateDataAsync(DTOOrderDetailsURequest dto)
        {
            OrderDetail? result = null;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_UpdateOrderDetail", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", dto.ID);
                    Command.Parameters.AddWithValue("@ItemID", dto.ItemID);
                    Command.Parameters.AddWithValue("@OrderID", dto.OrderID);
                    Command.Parameters.AddWithValue("@Quantity", dto.Quantity);

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

            await _Loader.LoadDataAsync(new List<OrderDetail> { result });
            return result;
        }
    }


    public class clsOrderDetailsRepository : IOrderDetailsRepository 
    {
        IOrderDetailsRepositoryReader _Read;
        IOrderDetailsRepositoryWriter _Write;
        public clsOrderDetailsRepository(IOrderDetailsRepositoryReader Read, IOrderDetailsRepositoryWriter Write)
        {
            _Read = Read;
            _Write = Write;
        }

        public async Task<List<OrderDetail>> GetAllDataAsync(List<int> Ids)
        {
            return await _Read.GetAllDataAsync(Ids);
        }

        public async Task<List<OrderDetail>> GetAllDataByOrderIDAsync(int orderID)
        {
            return await _Read.GetAllDataByOrderIDAsync(orderID);
        }
        public async Task<List<OrderDetail>> GetAllDataAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }

        public async Task<OrderDetail?> GetDataAsync(int ID)
        {
            return await _Read.GetDataAsync(ID);
        }

        public async Task<OrderDetail?> CreateDataAsync(DTOOrderDetailsCRequest Request)
        {
            return await _Write.CreateDataAsync(Request);
        }

        public async Task<OrderDetail?> UpdateDataAsync(DTOOrderDetailsURequest Request)
        {
            return await _Write.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await (_Write.DeleteDataAsync(ID));
        }
    }
}


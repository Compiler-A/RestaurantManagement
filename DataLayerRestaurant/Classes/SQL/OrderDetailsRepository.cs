using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.SQL
{

    public class OrderDetailsRepositoryReader : IOrderDetailsRepositoryReader
    {

        private readonly clsMySettings _Settings;
        public OrderDetailsRepositoryReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }
        public async Task<List<OrderDetail>> GetAllDataByOrderIdsAsync(List<int> Ids)
        {
            List<OrderDetail> result = new List<OrderDetail>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_GetAllOrderDetailsByOrderIds", Connection))
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
                            result.Add(OrderDetailMapper.ReaderToEntityResult(reader));
                        }
                    }
                }
            }
            return result;
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
                            result.Add(OrderDetailMapper.ReaderToEntityResult(reader));
                        }
                    }
                }
            }
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
                            result = (OrderDetailMapper.ReaderToEntityResult(Reader));
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
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
                            result.Add(OrderDetailMapper.ReaderToEntityResult(Reader));
                        }
                    }
                }
            }
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
                            result.Add(OrderDetailMapper.ReaderToEntityResult(Reader));
                        }
                    }
                }
            }
             return result;
        }
    }

    public class OrderDetailsRepositoryWriter : IOrderDetailsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        public OrderDetailsRepositoryWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value; 
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
                            result = OrderDetailMapper.ReaderToEntityResult(Reader);
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }

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
                            result = OrderDetailMapper.ReaderToEntityResult(Reader);
                        }
                    }
                }
            }
            if (result == null)
            {
                return null;
            }

             return result;
        }
    }


    public class OrderDetailsRepository : IOrderDetailsRepository 
    {
        IOrderDetailsRepositoryReader _Read;
        IOrderDetailsRepositoryWriter _Write;
        public OrderDetailsRepository(IOrderDetailsRepositoryReader Read, IOrderDetailsRepositoryWriter Write)
        {
            _Read = Read;
            _Write = Write;
        }

        public async Task<List<OrderDetail>> GetAllDataByOrderIdsAsync(List<int> Ids)
        {
            return await _Read.GetAllDataByOrderIdsAsync(Ids);
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


using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DataLayerRestaurant.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DomainLayer.Entities;

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

    public class clsOrderDetailsRepositoryReader : clsOrderDetailsRepositoryComposition ,IOrderDetailsRepositoryReader
    {

        private readonly clsMySettings _Settings;

        public clsOrderDetailsRepositoryReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
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
            return result;
        }
    }

    public class clsOrderDetailsRepositoryWriter : clsOrderDetailsRepositoryComposition,IOrderDetailsRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        public clsOrderDetailsRepositoryWriter(IOptions<clsMySettings> settings)
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
                            return (GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return null;
        }

        public async Task<OrderDetail?> UpdateDataAsync(DTOOrderDetailsURequest dto)
        {
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
                            return (GetDataFromDataBase(Reader));
                        }
                    }
                }
            }

            return null;
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


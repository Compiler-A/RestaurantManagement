using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Orders;

namespace DataLayerRestaurant.Classes
{
    
    public class clsCompositionDOrders : ICompositionDataBase<DTOOrders> 
    {
        public DTOOrders GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOOrders
            {
                ID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                TableID = reader.GetInt32(reader.GetOrdinal("TableID")),
                EmployerID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                StatusOrderID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
            };
        }
    }

    public class clsOrdersReader : clsCompositionDOrders, IReadableDOrders
    {
        private readonly clsMySettings _Settings;
        public clsOrdersReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<List<DTOOrders>> GetAllDataAsync(int page)
        {
            List<DTOOrders> result = new List<DTOOrders>();

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
            return result;
        }

        public async Task<DTOOrders?> GetDataAsync(int ID)
        {
            DTOOrders? result = null;

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
            return result;
        }

        public async Task<List<DTOOrders>?> GetFilterDataAsync(DTOOrderFilterRequest Request)
        {
            List<DTOOrders>? result = new List<DTOOrders>();

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
            return result;
        }
    }

    public class clsOrdersWriter :clsCompositionDOrders, IWritableDOrders
    {

        private readonly clsMySettings _Settings;
        public clsOrdersWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOOrders?> CreateDataAsync(DTOOrderCRequest order)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_AddOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TableID", order.TableID);
                    Command.Parameters.AddWithValue("@EmployeeID", order.EmployerID);
                    Command.Parameters.AddWithValue("@Date", order.OrderDate);
                    Command.Parameters.AddWithValue("@StatusID", order.StatusOrderID);
                    Command.Parameters.AddWithValue("@Amount", (object?)order.TotalAmount ?? DBNull.Value);


                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            return GetDataFromDataBase(Reader);
                        }
                    }

                }
            }

            return null;
        }

        public async Task<DTOOrders?> UpdateDataAsync(DTOOrderURequest order)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_UpdateOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TableID", order.TableID);
                    Command.Parameters.AddWithValue("@EmployeeID", order.EmployerID);
                    Command.Parameters.AddWithValue("@Date", order.OrderDate);
                    Command.Parameters.AddWithValue("@StatusID", order.StatusOrderID);
                    Command.Parameters.AddWithValue("@Amount", (object?)order.TotalAmount ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@ID", order.OrderID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            return GetDataFromDataBase(Reader);
                        }
                    }
                }
            }
            return null;
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

    public class clsDataOrders : IDataOrders
    {
        IWritableDOrders _Write;
        IReadableDOrders _Read;

        public clsDataOrders(IWritableDOrders write, IReadableDOrders read)
        {
            _Write = write;
            _Read = read;
        }

        public async Task<List<DTOOrders>> GetAllOrdersAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }
        public async Task<DTOOrders?> GetOrderAsync(int ID)
        {
            return await _Read.GetDataAsync(ID);
        }
        public async Task<List<DTOOrders>?> GetFilterOrderAsync(DTOOrderFilterRequest Request)
        {
            return await _Read.GetFilterDataAsync(Request);
        }

        public async Task<DTOOrders?> AddOrderAsync(DTOOrderCRequest order)
        {
            return await _Write.CreateDataAsync(order);
        }
        public async Task<DTOOrders?> UpdateOrderAsync(DTOOrderURequest order)
        {
            return await _Write.UpdateDataAsync(order);
        }
        public async Task<bool> DeleteOrderAsync(int ID)
        {
            return await _Write.DeleteDataAsync(ID);
        }
    }
}
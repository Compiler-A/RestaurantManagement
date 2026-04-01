using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayerRestaurant
{
    public class DTOOrderCRequest
    {
        public int TableID { get; set; }
        public int EmployerID { get; set; }
        public int StatusOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public DTOOrderCRequest(int tableID, int employerID, int statusOrderID, DateTime orderDate, decimal? totalAmount)
        {
            TableID = tableID;
            EmployerID = employerID;
            StatusOrderID = statusOrderID;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
        }
    }

    public class DTOOrderFilterRequest
    {
        public int Page { get; set; }
        public int TableID { get; set; }
        public int EmployeeID { get; set; }
        public int StatusOrderID { get; set; }
        public DTOOrderFilterRequest(int page, int tableID, int employeeID, int statusOrderID)
        {
            Page = page;
            TableID = tableID;
            EmployeeID = employeeID;
            StatusOrderID = statusOrderID;
        }
    }

    public class DTOOrderURequest : DTOOrderCRequest
    {
        public int OrderID { get; set; }

        public DTOOrderURequest(int orderID, int tableID, int employerID, int statusOrderID, DateTime orderDate, decimal? totalAmount)
            : base(tableID, employerID, statusOrderID, orderDate, totalAmount)
        {
            OrderID = orderID;
        }
    }

    public class DTOOrders
    {
        public int ID { get; set; }
        public int TableID { get; set; }
        public int EmployerID { get; set; }
        public int StatusOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }


        public DTOTables? tables { get; set; }
        public DTOEmployees? employees { get; set; }
        public DTOStatusOrders? statusOrders { get; set; }


        public DTOOrders(int iD, int tableID, int employerID, int statusOrderID, DateTime orderDate, decimal? totalAmount)
        {
            ID = iD;
            TableID = tableID;
            EmployerID = employerID;
            StatusOrderID = statusOrderID;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
        }
        public DTOOrders()
        {
            ID = -1;
            TableID = -1;
            EmployerID = -1;
            StatusOrderID = -1;
            OrderDate = DateTime.MinValue;
            TotalAmount = null;
        }
    }

    

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

    public class clsReadableDOrders : clsCompositionDOrders, IReadableDOrders
    {
        public async Task<List<DTOOrders>> GetAllDataAsync(int page)
        {
            List<DTOOrders> result = new List<DTOOrders>();

            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_GetAllOrders", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

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

            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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

            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Orders.SP_GetFilterOrders", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", Request.Page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);
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

    public class clsWritableDOrders :clsCompositionDOrders, IWritableDOrders
    {
        public async Task<DTOOrders?> CreateDataAsync(DTOOrderCRequest order)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

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

    public interface IReadableOrdersData
    {
        Task<List<DTOOrders>> GetAllOrdersAsync(int page);
        Task<DTOOrders?> GetOrderAsync(int ID);
    }
    public interface IWritableOrdersData
    {
        Task<int> Add(DTOOrders order);
        Task<bool> Update(DTOOrders order);
        Task<bool> Delete(int id);
    }

    public interface IDataOrders : IWritableOrdersData, IReadableOrdersData
    {
    }

    public class clsDataOrders : IDataOrders
    {

        private DTOOrders _GetOrderFromDataBase(SqlDataReader reader)
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
        public async Task<List<DTOOrders>> GetAllOrdersAsync(int page)
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
                            result.Add(_GetOrderFromDataBase(Reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<DTOOrders?> GetOrderAsync(int ID)
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
                            result = _GetOrderFromDataBase(Reader);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Add(DTOOrders order)
        {
            int NewID = -1;
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

                    SqlParameter output = new SqlParameter("@NewID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(output);

                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    if (output.Value != null && output.Value != DBNull.Value)
                        NewID  = (int)output.Value;
                   
                }
            }

            return NewID;
        }

        public async Task<bool> Update(DTOOrders order)
        {
            bool Update = false;
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
                    Command.Parameters.AddWithValue("@ID", order.ID);

                    await Connection.OpenAsync();
                    Update = await Command.ExecuteNonQueryAsync() > 0;
                }
            }
            return Update;
        }

        public async Task<bool> Delete(int ID)
        {
            bool Delete = false;
            using (SqlConnection Connection =  new SqlConnection(clsDataAccessLayer.ConnectionString))
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
}

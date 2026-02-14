using Azure;
using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public class DTOOrderDetails
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public DTOOrders? Order { get; set; }
        public DTOMenuItem? Item { get; set; }

        public DTOOrderDetails() 
        {
            ID = -1;
            OrderID = -1;
            ItemID = -1;
            Quantity = -1;
            SubTotal = -1;
        }
        public DTOOrderDetails(int iD, int orderID, int itemID, int quantity, decimal subTotal)
        {
            ID = iD;
            OrderID = orderID;
            ItemID = itemID;
            Quantity = quantity;
            SubTotal = subTotal;
        }
    }

    public interface IReadableOrderDetailsData
    {
        Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page);
        Task<DTOOrderDetails?> GetOrderDetailAsync(int ID);
    }
    public interface IWritableOrderDetailsData
    {
        Task<int> AddAsync(DTOOrderDetails orderDetails);
        Task<bool> UpdateAsync(DTOOrderDetails orderDetails);
        Task<bool> DeleteAsync(int id);
    }

    public interface IDataOrderDetails : IReadableOrderDetailsData, IWritableOrderDetailsData
    {
    }

    public class clsDataOrderDetails : IDataOrderDetails 
    {
        private DTOOrderDetails _GetOrderFromDataBase(SqlDataReader reader)
        {
            return new DTOOrderDetails
            {
                ID = reader.GetInt32(reader.GetOrdinal("OrderDetailID")),
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                SubTotal = reader.GetDecimal(reader.GetOrdinal("SubTotal"))
            };
        }
        public async Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page)
        {
            List<DTOOrderDetails> result = new List<DTOOrderDetails>();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_GetAllOrderDetails", Connection))
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

        public async Task<DTOOrderDetails?> GetOrderDetailAsync(int ID)
        {
            DTOOrderDetails? result = null;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_GetOrderDetailByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if(await Reader.ReadAsync())
                        {
                            result = (_GetOrderFromDataBase(Reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> AddAsync(DTOOrderDetails orderDetails)
        {
            int NewID = -1;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_AddOrderDetail", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ItemID", orderDetails.ItemID);
                    Command.Parameters.AddWithValue("@OrderID", orderDetails.OrderID);
                    Command.Parameters.AddWithValue("@Quantity", orderDetails.Quantity);
                    SqlParameter outputID = new SqlParameter("@NewID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(outputID);
                    SqlParameter outputTotal = new SqlParameter("@Total", System.Data.SqlDbType.Decimal)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(outputTotal);
                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    if (outputID.Value != null && outputID.Value != DBNull.Value)
                        NewID = (int)outputID.Value;

                    if (outputTotal.Value != null && outputTotal.Value != DBNull.Value)
                        orderDetails.SubTotal = (decimal)outputTotal.Value;
                }
            }

            return NewID;
        }

        public async Task<bool> UpdateAsync(DTOOrderDetails orderDetails)
        {
            bool Update = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_UpdateOrderDetail", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", orderDetails.ID);
                    Command.Parameters.AddWithValue("@ItemID", orderDetails.ItemID);
                    Command.Parameters.AddWithValue("@OrderID", orderDetails.OrderID);
                    Command.Parameters.AddWithValue("@Quantity", orderDetails.Quantity);
                    SqlParameter outputTotal = new SqlParameter("@Total", System.Data.SqlDbType.Decimal)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(outputTotal);
                    await Connection.OpenAsync();
                    Update = await Command.ExecuteNonQueryAsync() > 0;
                   
                    if (outputTotal.Value != null && outputTotal.Value != DBNull.Value)
                        orderDetails.SubTotal = (decimal)outputTotal.Value;
                }
            }

            return Update;
        }
        public async Task<bool> DeleteAsync(int ID)
        {
            bool Delete = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("OrderDetails.SP_DeleteOrderDetail", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    Delete = await Command.ExecuteNonQueryAsync() > 0;
                }

                return Delete;
            }
        }
    }
}


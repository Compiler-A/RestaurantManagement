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

    public class DTOOrderDetailsCRequest
    {
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public DTOOrderDetailsCRequest(int orderID, int itemID, int quantity, decimal subTotal)
        {
            OrderID = orderID;
            ItemID = itemID;
            Quantity = quantity;
            SubTotal = subTotal;
        }
    }

    public class DTOOrderDetailsURequest : DTOOrderDetailsCRequest
    {
        public int ID { get; set; }

        public DTOOrderDetailsURequest
            (int iD, int orderID, int itemID, int quantity, decimal subTotal) 
            : base(orderID, itemID, quantity, subTotal)
        {
            ID = iD;
            OrderID = orderID;
            ItemID = itemID;
            Quantity = quantity;
            SubTotal = subTotal;
        }
    }


    public class DTOOrderDetails
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public DTOOrders? Order { get; set; }
        public DTOMenuItems? Item { get; set; }

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

    

    public class clsCompositionDOrderDetails : ICompositionDataBase<DTOOrderDetails>
    {
        public DTOOrderDetails GetDataFromDataBase(SqlDataReader reader)
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
    }

    public class clsReadableDOrderDetails : clsCompositionDOrderDetails ,IReadableDOrderDetails
    {
        public async Task<DTOOrderDetails?> GetDataAsync(int ID)
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
                        if (await Reader.ReadAsync())
                        {
                            result = (GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<DTOOrderDetails>> GetAllDataAsync(int page)
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
                            result.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<DTOOrderDetails>> GetAllDataByOrderIDAsync(int orderID)
        {
            List<DTOOrderDetails> result = new List<DTOOrderDetails>();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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

    public class clsWritableDOrderDetails : clsCompositionDOrderDetails,IWritableDOrderDetails
    {
        public async Task<bool> DeleteDataAsync(int id)
        {
            bool Delete = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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
        public async Task<DTOOrderDetails?> CreateDataAsync(DTOOrderDetailsCRequest dto)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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

        public async Task<DTOOrderDetails?> UpdateDataAsync(DTOOrderDetailsURequest dto)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
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


    public class clsDataOrderDetails : IDataOrderDetails 
    {
        IReadableDOrderDetails _Read;
        IWritableDOrderDetails _Write;
        public clsDataOrderDetails(IReadableDOrderDetails Read, IWritableDOrderDetails Write)
        {
            _Read = Read;
            _Write = Write;
        }

        public async Task<List<DTOOrderDetails>> GetAllOrderDetailsByOrderIDAsync(int orderID)
        {
            return await _Read.GetAllDataByOrderIDAsync(orderID);
        }
        public async Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }

        public async Task<DTOOrderDetails?> GetOrderDetailAsync(int ID)
        {
            return await _Read.GetDataAsync(ID);
        }

        public async Task<DTOOrderDetails?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request)
        {
            return await _Write.CreateDataAsync(Request);
        }

        public async Task<DTOOrderDetails?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request)
        {
            return await _Write.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteOrderDetailAsync(int ID)
        {
            return await (_Write.DeleteDataAsync(ID));
        }
    }
}


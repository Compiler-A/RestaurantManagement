using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.OrderDetails;

namespace DataLayerRestaurant.Classes
{
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

    public class clsOrderDetailsReader : clsCompositionDOrderDetails ,IReadableDOrderDetails
    {

        private readonly clsMySettings _Settings;

        public clsOrderDetailsReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<DTOOrderDetails?> GetDataAsync(int ID)
        {
            DTOOrderDetails? result = null;
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
        public async Task<List<DTOOrderDetails>> GetAllDataAsync(int page)
        {
            List<DTOOrderDetails> result = new List<DTOOrderDetails>();
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
        public async Task<List<DTOOrderDetails>> GetAllDataByOrderIDAsync(int orderID)
        {
            List<DTOOrderDetails> result = new List<DTOOrderDetails>();
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

    public class clsOrderDetailsWriter : clsCompositionDOrderDetails,IWritableDOrderDetails
    {
        private readonly clsMySettings _Settings;
        public clsOrderDetailsWriter(IOptions<clsMySettings> settings)
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
        public async Task<DTOOrderDetails?> CreateDataAsync(DTOOrderDetailsCRequest dto)
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

        public async Task<DTOOrderDetails?> UpdateDataAsync(DTOOrderDetailsURequest dto)
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


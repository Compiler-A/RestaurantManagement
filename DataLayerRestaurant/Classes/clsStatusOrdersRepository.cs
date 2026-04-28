using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DataLayerRestaurant.Interfaces;


namespace DataLayerRestaurant.Classes
{    

    public class  clsStatusOrdersRepositoryComposition : ICompositionDataBase<StatusOrder>
    {
        public StatusOrder GetDataFromDataBase(SqlDataReader reader)
        {
            return new StatusOrder
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            };
        }
    }
    public class clsStatusOrdersRepositoryReader : clsStatusOrdersRepositoryComposition, IStatusOrdersRepositoryReader
    {
        public readonly clsMySettings _Settings;

        public clsStatusOrdersRepositoryReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<StatusOrder?> GetDataAsync(int ID)
        {
            StatusOrder? statusOrder = null;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_GetStatusOrderByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            statusOrder = GetDataFromDataBase(Reader);
                        }
                    }
                }
            }
            return statusOrder;
        }
        public async Task<List<StatusOrder>> GetAllDataAsync(int Page)
        {
            List<StatusOrder> List = new List<StatusOrder>();

            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_GetAllStatusOrders", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", Page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            List.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return List;
        }
    }
    public class clsStatusOrdersRepositoryWriter : clsStatusOrdersRepositoryComposition, IStatusOrdersRepositoryWriter
    {

        private readonly clsMySettings _Settings;

        public clsStatusOrdersRepositoryWriter(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<StatusOrder?> CreateDataAsync(DTOStatusOrdersCRequest Request)
        {

            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_AddStatusOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", Request.Name);
                    

                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
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
        public async Task<StatusOrder?> UpdateDataAsync(DTOStatusOrdersURequest Request)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_UpdateStatusOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", Request.Name);
                    Command.Parameters.AddWithValue("@ID", Request.ID);

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
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_DeleteStatusOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    Deleted = (await Command.ExecuteNonQueryAsync()) > 0;
                }
            }
            return Deleted;
        }
    }

    public class clsStatusOrdersRepository : IStatusOrdersRepository
    {
        IStatusOrdersRepositoryReader _Read;
        IStatusOrdersRepositoryWriter _Write;
        public clsStatusOrdersRepository(IStatusOrdersRepositoryReader Read, IStatusOrdersRepositoryWriter Write)
        {
            _Read = Read;
            _Write = Write;
        }

        public async Task<StatusOrder?> GetStatusOrderAsync(int id)
        {
            return await _Read.GetDataAsync(id);
        }
        public async Task<List<StatusOrder>> GetAllStatusOrdersAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }
        public async Task<StatusOrder?> AddStatusOrderAsync(DTOStatusOrdersCRequest Request)
        {
            return await _Write.CreateDataAsync(Request);
        }
        public async Task<StatusOrder?> UpdateStatusOrderAsync(DTOStatusOrdersURequest Request)
        {
            return await _Write.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteStatusOrderAsync(int id)
        {
            return await _Write.DeleteDataAsync(id);
        }
    }
}

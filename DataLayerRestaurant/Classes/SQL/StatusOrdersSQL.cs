using ContractsLayerRestaurant.DTORequest.StatusOrders;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;


namespace DataLayerRestaurant.Classes.SQL
{    


    public class StatusOrdersRepositoryReader : IStatusOrdersRepositoryReader
    {
        private readonly clsMySettings _Settings;

        public StatusOrdersRepositoryReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<List<StatusOrder>> GetAllDataAsync(List<int> Ids)
        {
            List<StatusOrder> result = new List<StatusOrder>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_GetAllStatusOrdersByIds", Connection))
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
                            result.Add(StatusOrderMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
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
                            statusOrder = StatusOrderMapper.ReaderToEntity(Reader);
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
                            List.Add(StatusOrderMapper.ReaderToEntity(Reader));
                        }
                    }
                }
            }
            return List;
        }
    }
    public class StatusOrdersRepositoryWriter : IStatusOrdersRepositoryWriter
    {

        private readonly clsMySettings _Settings;

        public StatusOrdersRepositoryWriter(IOptions<clsMySettings> Settings)
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
                            return StatusOrderMapper.ReaderToEntity(Reader);
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
                            return StatusOrderMapper.ReaderToEntity(Reader);
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

}

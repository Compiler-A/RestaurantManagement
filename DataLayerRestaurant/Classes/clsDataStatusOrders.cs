using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DataLayerRestaurant
{

    public class DTOStatusOrdersCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public DTOStatusOrdersCRequest(string statusName) 
        {
            this.Name = statusName;
        }
    }

    public class DTOStatusOrdersURequest : DTOStatusOrdersCRequest
    {

        [Range(1,int.MaxValue,ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }

        public DTOStatusOrdersURequest(int ID, string statusOrderName) : base(statusOrderName)
        {
            this.ID = ID;
        }
    }

    public class DTOStatusOrders
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public DTOStatusOrders()
        {
            ID = -1;
            Name = string.Empty;
        }
        public DTOStatusOrders(int idStatusOrder, string statusOrderName)
        {
            this.ID = idStatusOrder;
            this.Name = statusOrderName;
        }
    }

    

    public class  clsCompositionDStatusOrders : ICompositionDataBase<DTOStatusOrders>
    {
        public DTOStatusOrders GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOStatusOrders
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            };
        }
    }
    public class clsStatusOrdersReader : clsCompositionDStatusOrders, IReadableDStatusOrders
    {
        public readonly clsMySettings _Settings;

        public clsStatusOrdersReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<DTOStatusOrders?> GetDataAsync(int ID)
        {
            DTOStatusOrders? statusOrder = null;
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
        public async Task<List<DTOStatusOrders>> GetAllDataAsync(int Page)
        {
            List<DTOStatusOrders> List = new List<DTOStatusOrders>();

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
    public class clsStatusOrdersWriter : clsCompositionDStatusOrders, IWritableDStatusOrders
    {

        private readonly clsMySettings _Settings;

        public clsStatusOrdersWriter(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<DTOStatusOrders?> CreateDataAsync(DTOStatusOrdersCRequest Request)
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
        public async Task<DTOStatusOrders?> UpdateDataAsync(DTOStatusOrdersURequest Request)
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

    public class clsDataStatusOrders : IDataStatusOrders
    {
        IReadableDStatusOrders _Read;
        IWritableDStatusOrders _Write;
        public clsDataStatusOrders(IReadableDStatusOrders Read, IWritableDStatusOrders Write)
        {
            _Read = Read;
            _Write = Write;
        }

        public async Task<DTOStatusOrders?> GetStatusOrderAsync(int id)
        {
            return await _Read.GetDataAsync(id);
        }
        public async Task<List<DTOStatusOrders>> GetAllStatusOrdersAsync(int page)
        {
            return await _Read.GetAllDataAsync(page);
        }
        public async Task<DTOStatusOrders?> AddStatusOrderAsync(DTOStatusOrdersCRequest Request)
        {
            return await _Write.CreateDataAsync(Request);
        }
        public async Task<DTOStatusOrders?> UpdateStatusOrderAsync(DTOStatusOrdersURequest Request)
        {
            return await _Write.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteStatusOrderAsync(int id)
        {
            return await _Write.DeleteDataAsync(id);
        }
    }
}

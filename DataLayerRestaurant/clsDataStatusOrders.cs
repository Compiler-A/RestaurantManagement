using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public class DTOStatusOrders
    {
        public int idStatusOrder { get; set; }
        public string statusOrderName { get; set; }

        public DTOStatusOrders()
        {
            idStatusOrder = -1;
            statusOrderName = string.Empty;
        }
        public DTOStatusOrders(int idStatusOrder, string statusOrderName)
        {
            this.idStatusOrder = idStatusOrder;
            this.statusOrderName = statusOrderName;
        }
    }


    public interface IReadableStatusOrdersData
    {
        Task<DTOStatusOrders?> GetStatusOrderByID(int id);
        Task<List<DTOStatusOrders>> GetAllStatusOrders(int page);
    }

    public interface IWritableStatusOrdersData
    {
        Task<int> AddStatusOrder(DTOStatusOrders statusOrder);
        Task<bool> UpdateStatusOrder(DTOStatusOrders statusOrder);
        Task<bool> DeleteStatusOrder(int id);
    }
    public interface IDataStatusOrders : IReadableStatusOrdersData, IWritableStatusOrdersData { }

    public class clsDataStatusOrders : IDataStatusOrders
    {
        private  DTOStatusOrders _GetStatusOrderFromDataBase(SqlDataReader reader)
        {
            return new DTOStatusOrders
            {
                idStatusOrder = reader.GetInt32(reader.GetOrdinal("StatusOrderID")),
                statusOrderName = reader.GetString(reader.GetOrdinal("Name"))
            };
        }

        public async Task<DTOStatusOrders?> GetStatusOrderByID(int id)
        {
            DTOStatusOrders? statusOrder = null;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_GetStatusOrderByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", id);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            statusOrder = _GetStatusOrderFromDataBase(Reader);
                        }
                    }
                }
            }
            return statusOrder;
        }
        public async Task<List<DTOStatusOrders>> GetAllStatusOrders(int page)
        {
            List<DTOStatusOrders> List = new List<DTOStatusOrders>();

            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_GetAllStatusOrders", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            List.Add(_GetStatusOrderFromDataBase(Reader));
                        }
                    }
                }
            }
            return List;
        }
        public async Task<int> AddStatusOrder(DTOStatusOrders statusOrder)
        {
            int newID = -1;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_AddStatusOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", statusOrder.statusOrderName);
                    SqlParameter outputParameter = new SqlParameter("@NewID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(outputParameter);

                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    if (outputParameter.Value != null && outputParameter.Value != DBNull.Value)
                        newID = Convert.ToInt32(outputParameter.Value);
                }
            }

            return newID;
        }
        public async Task<bool> UpdateStatusOrder(DTOStatusOrders statusOrder)
        {
            bool Updated = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_UpdateStatusOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", statusOrder.statusOrderName);
                    Command.Parameters.AddWithValue("@ID", statusOrder.idStatusOrder);

                    await Connection.OpenAsync();
                    Updated = (await Command.ExecuteNonQueryAsync()) > 0;
                }
            }
            return Updated; 
        }
        public async Task<bool> DeleteStatusOrder(int id)
        {
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusOrders.SP_DeleteStatusOrder", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", id);

                    await Connection.OpenAsync();
                    Deleted = (await Command.ExecuteNonQueryAsync()) > 0;
                }
            }
            return Deleted;
        }
    }
}

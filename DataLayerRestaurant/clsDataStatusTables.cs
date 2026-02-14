using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public class DTOStatusTables
    {
        public int StatusTableID { get; set; }
        public string StatusTableName { get; set; }

        public DTOStatusTables()
        {
            StatusTableID = -1;
            StatusTableName = string.Empty;
        }
        public DTOStatusTables(int statusTableID, string statusTableName)
        {
            StatusTableID = statusTableID;
            StatusTableName = statusTableName;
        }
    }

    public interface IReadableStatusTables
    {
        Task<DTOStatusTables?> GetStatusTableID(int id);
        Task<List<DTOStatusTables>> GetAllStatustables(int page);
        Task<bool> isFindStatus(int id);
    }

    public interface IWritableStatusTables
    {
        Task<int> AddStatusTable(DTOStatusTables StatusTable);
        Task<bool> UpdateStatusTable(DTOStatusTables StatusTable);
        Task<bool> DeleteStatusTable(int id);
    }

    

    public interface IDataStatusTables : IReadableStatusTables, IWritableStatusTables
    { }

    public class clsDataStatusTables : IDataStatusTables
    {

        private static async Task<DTOStatusTables> MapReaderToMenuItem(SqlDataReader reader)
        {
            DTOStatusTables menuItem = new DTOStatusTables
            {
                StatusTableID = reader.GetInt32(reader.GetOrdinal("StatusTableID")),
                StatusTableName = reader.GetString(reader.GetOrdinal("Name"))
            };
            return menuItem;
        }

        public async Task<bool> isFindStatus(int id)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_IsFind", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", id);
                    SqlParameter OutputParameter = new SqlParameter("@Found", System.Data.SqlDbType.Bit)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(OutputParameter);
                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    return OutputParameter.Value != DBNull.Value && (bool)OutputParameter.Value;
                }
            }

        }
        public async Task<DTOStatusTables?> GetStatusTableID(int id)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_GetStatusTableByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", id);
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            return await MapReaderToMenuItem(Reader);
                        }
                    }
                }
            }
            return null;
        }

         public async Task<List<DTOStatusTables>> GetAllStatustables(int page)
        {
            List<DTOStatusTables> L = new List<DTOStatusTables>();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_GetAllStatusTables", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            DTOStatusTables menuItem = await MapReaderToMenuItem(Reader);
                            L.Add(menuItem);
                        }
                    }
                }

            }
            return L;
        }
        public async Task<int> AddStatusTable(DTOStatusTables StatusTable)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_AddStatusTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", StatusTable.StatusTableName);
                    SqlParameter OutputParameter = new SqlParameter("@ID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(OutputParameter);

                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    return OutputParameter.Value != DBNull.Value ? (int)OutputParameter.Value : -1;
                }
            }
        }


        public async Task<bool> UpdateStatusTable(DTOStatusTables StatusTable)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_UpdateStatusTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", StatusTable.StatusTableName);
                    Command.Parameters.AddWithValue("@ID", StatusTable.StatusTableID);

                    await Connection.OpenAsync();
                    int Count = await Command.ExecuteNonQueryAsync();
                    return Count > 0;
                }
            }
        }
        public async Task<bool> DeleteStatusTable(int id)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_DeleteStatusTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", id);

                    await Connection.OpenAsync();
                    int Count = await Command.ExecuteNonQueryAsync();
                    return Count > 0;
                }
            }
        }
    }
}

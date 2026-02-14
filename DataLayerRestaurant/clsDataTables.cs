using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public class DTOTables
    {
        public int ID {  get; set; }
        public string Table { get; set; }
        public int Seats { get; set; }
        public int StatusTableID { get; set; }
        public DTOStatusTables? StatusTable { get; set; }

        public DTOTables()
        {
            ID = -1;
            Table = string.Empty;
            Seats = -1;
            StatusTable = null;
            StatusTableID = -1;
        }
        public DTOTables(int iD, string table, int seats, int statusTableID)
        {
            ID = iD;
            Table = table;
            Seats = seats;
            StatusTableID = statusTableID;
        }
    }

    public interface IReadableTables
    {
        Task<DTOTables?> GetTableID(int id);
        Task<List<DTOTables>> GetAlltables(int page);
    }

    public interface IWritableTables
    {
        Task<(int, string)> AddTable(DTOTables Table);
        Task<(bool, string)> UpdateTable(DTOTables Table);
        Task<bool> DeleteTable(int id);
    }



    public interface IDataTables : IReadableTables, IWritableTables
    { }
    public class clsDataTables : IDataTables
    {
        private async Task<DTOTables> MapReader(SqlDataReader Reader)
        {
            DTOTables table = new DTOTables
            {
                ID = Reader.GetInt32(Reader.GetOrdinal("TableID")),
                Table = Reader.GetString(Reader.GetOrdinal("TableNumber")),
                Seats = Reader.GetInt32(Reader.GetOrdinal("Seats")),
                StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID"))
            };
            return table;
        }
        public async Task<DTOTables?> GetTableID(int ID)
        {
            DTOTables? table = new DTOTables();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetTableByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TableID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        table = (await Reader.ReadAsync()) ? await MapReader(Reader) : null;
                    }
                }
            }
            return table;
        }
        public async Task<List<DTOTables>> GetAlltables(int page)
        {
            List<DTOTables> listTables = new List<DTOTables>();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetAllTables", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            listTables.Add(await MapReader(Reader));
                        }
                    }
                }
            }
            return listTables;
        }
        public async Task<(int,string)> AddTable(DTOTables Tables)
        {
            int Result = -1;
            string TableNumber = string.Empty;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_AddTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@StatusTableID", Tables.StatusTableID);
                    Command.Parameters.AddWithValue("@Seats", Tables.Seats);
                    SqlParameter outputParameter = new SqlParameter("@ID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    SqlParameter outputParameter2 = new SqlParameter("@TableNumber", System.Data.SqlDbType.NVarChar,50)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(outputParameter);
                    Command.Parameters.Add(outputParameter2);

                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();
                    if (outputParameter.Value != null && outputParameter.Value != DBNull.Value)
                    {
                        Result = (int)outputParameter.Value;
                        if (outputParameter2.Value != null && outputParameter2.Value != DBNull.Value)
                        {
                            TableNumber = (string)outputParameter2.Value;
                        }
                    }
                }
            }
            return (Result, TableNumber);
        }
        public async Task<(bool, string)> UpdateTable(DTOTables Table)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_UpdateTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", Table.ID);
                    Command.Parameters.AddWithValue("@StatusTableID", Table.StatusTableID);
                    Command.Parameters.AddWithValue("@Seats", Table.Seats);
                    SqlParameter outputParameter = new SqlParameter("@TableNumber", System.Data.SqlDbType.NVarChar, 50)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(outputParameter);
                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    return (rowsAffected > 0, (string)outputParameter.Value);
                }
            }
        }
        public async Task<bool> DeleteTable(int ID)
        {  
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_DeleteTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);
                    await Connection.OpenAsync();
                    int rowsAffected = await Command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

    }
}

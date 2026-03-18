using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public class DTOStatusTablesCRequest
    {
        public string Name { get; set; }
        public DTOStatusTablesCRequest(string Name)
        {
            this.Name = Name; 
        }
    }

    public class DTOStatusTablesURequest : DTOStatusTablesCRequest
    {
        public int ID { get; set; }

        public DTOStatusTablesURequest(int ID,  string Name) : base(Name)
        {
            this.ID = ID;
        }
    }

    public class DTOStatusTables
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public DTOStatusTables()
        {
            ID = -1;
            Name = string.Empty;
        }
        public DTOStatusTables(int statusTableID, string statusTableName)
        {
            ID = statusTableID;
            Name = statusTableName;
        }
    }

    public class clsCompositionDStatusTables : ICompositionDataBase<DTOStatusTables>
    {
        public DTOStatusTables GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOStatusTables
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusTableID")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            };
        }
    }

    public class clsReadableDStatusTables : clsCompositionDStatusTables, IReadableDStatusTables
    {
        public async Task<bool> isFindDataAsync(int id)
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
        public async Task<DTOStatusTables?> GetDataAsync(int id)
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
                            return GetDataFromDataBase(Reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<DTOStatusTables>> GetAllDataAsync(int page)
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
                            DTOStatusTables menuItem = GetDataFromDataBase(Reader);
                            L.Add(menuItem);
                        }
                    }
                }

            }
            return L;
        }
    }


    public class clsWritableDStatusTables : clsCompositionDStatusTables, IWritableDStatusTables
    {
        public async Task<DTOStatusTables?> CreateDataAsync(DTOStatusTablesCRequest StatusTable)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_AddStatusTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", StatusTable.Name);
                    SqlParameter OutputParameter = new SqlParameter("@ID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(OutputParameter);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            return GetDataFromDataBase(Reader);
                        }
                    }
                    return null;
                }
            }
        }


        public async Task<DTOStatusTables?> UpdateDataAsync(DTOStatusTablesURequest StatusTable)
        {
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_UpdateStatusTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", StatusTable.Name);
                    Command.Parameters.AddWithValue("@ID", StatusTable.ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            return GetDataFromDataBase(Reader);
                        }
                    }
                    return null;
                }
            }
        }
        public async Task<bool> DeleteDataAsync(int id)
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

    public class clsDataStatusTables : IDataStatusTables
    {
        private IReadableDStatusTables _IRead;
        private IWritableDStatusTables _IWrite;
        public clsDataStatusTables(IReadableDStatusTables Read, IWritableDStatusTables Write) 
        {
            _IRead = Read;
            _IWrite = Write;
        }

        public async Task<bool> isFindAsync(int id)
        {
            return await _IRead.isFindDataAsync(id);

        }
        public async Task<DTOStatusTables?> GetStatuTableAsync(int id)
        {
            return await _IRead.GetDataAsync(id);
        }

        public async Task<List<DTOStatusTables>> GetAllStatustablesAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }
        public async Task<DTOStatusTables?> AddStatusTableAsync(DTOStatusTablesCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }
        public async Task<DTOStatusTables?> UpdateStatusTableAsync(DTOStatusTablesURequest Request)
        {
           return await _IWrite.UpdateDataAsync(Request);
        }
        public async Task<bool> DeleteStatusTableAsync(int id)
        {
            return await _IWrite.DeleteDataAsync(id);
        }
    }
}

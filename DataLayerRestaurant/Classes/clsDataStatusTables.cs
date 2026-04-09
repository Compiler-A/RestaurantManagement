using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using ContractsLayerRestaurant.DTOs.StatusTables;
using DataLayerRestaurant.Interfaces;

namespace DataLayerRestaurant.Classes
{

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

    public class clsStatusTablesReader : clsCompositionDStatusTables, IReadableDStatusTables
    {

        private readonly clsMySettings _Settings;

        public clsStatusTablesReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<bool> isFindDataAsync(int id)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
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
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
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
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_GetAllStatusTables", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

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


    public class clsStatusTablesWriter : clsCompositionDStatusTables, IWritableDStatusTables
    {


        readonly private clsMySettings _Settings;

        public clsStatusTablesWriter(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<DTOStatusTables?> CreateDataAsync(DTOStatusTablesCRequest StatusTable)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
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
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
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
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
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

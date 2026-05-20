using ContractsLayerRestaurant.DTORequest.StatusTables;
using ContractsLayerRestaurant.Interfaces.Repositories;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataLayerRestaurant.Classes.SQL
{


    public class StatusTablesRepositoryReader : IStatusTablesRepositoryReader
    {

        private readonly clsMySettings _Settings;

        public StatusTablesRepositoryReader(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<List<StatusTable>> GetAllDataAsync(List<int> Ids)
        {
            List<StatusTable> result = new List<StatusTable>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("StatusTables.SP_GetAllStatusTablesByIds", Connection))
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
                            result.Add(StatusTableMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
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
        public async Task<StatusTable?> GetDataAsync(int id)
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
                            return StatusTableMapper.ReaderToEntity(Reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<StatusTable>> GetAllDataAsync(int page)
        {
            List<StatusTable> L = new List<StatusTable>();
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
                            StatusTable menuItem = StatusTableMapper.ReaderToEntity(Reader);
                            L.Add(menuItem);
                        }
                    }
                }

            }
            return L;
        }
    }


    public class StatusTablesRepositoryWriter : IStatusTablesRepositoryWriter
    {


        readonly private clsMySettings _Settings;

        public StatusTablesRepositoryWriter(IOptions<clsMySettings> Settings)
        {
            _Settings = Settings.Value;
        }

        public async Task<StatusTable?> CreateDataAsync(DTOStatusTablesCRequest StatusTable)
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
                            return StatusTableMapper.ReaderToEntity(Reader);
                        }
                    }
                    return null;
                }
            }
        }


        public async Task<StatusTable?> UpdateDataAsync(DTOStatusTablesURequest StatusTable)
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
                            return StatusTableMapper.ReaderToEntity(Reader);
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
}

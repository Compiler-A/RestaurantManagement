using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.Tables;
using DataLayerRestaurant.Interfaces;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Classes
{

    public class clsTablesRepositoryComposition : ICompositionDataBase<Table>
    {
        public Table GetDataFromDataBase(SqlDataReader Reader)
        {
            return new Table
            {
                ID = Reader.GetInt32(Reader.GetOrdinal("TableID")),
                Name = Reader.GetString(Reader.GetOrdinal("TableNumber")),
                Seats = Reader.GetInt32(Reader.GetOrdinal("Seats")),
                StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID"))
            };
        }
    }

    public class clsTablesRepositoryReader : clsTablesRepositoryComposition ,ITablesRepositoryReader
    {

        private readonly clsMySettings _Settings;
        public clsTablesRepositoryReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<List<Table>> GetAllDataAvailablesAsync()
        {
            List<Table> listTables = new List<Table>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetTablesAvailables", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            listTables.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return listTables;
        }
        public async Task<List<Table>> GetAllDataAsync()
        {
            List<Table> listTables = new List<Table>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetAllTablesNoPagination", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            listTables.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return listTables;
        }
        public async Task<Table?> GetDataAsync(int ID)
        {
            Table? table = new Table();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetTableByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TableID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        table = (await Reader.ReadAsync()) ? GetDataFromDataBase(Reader) : null;
                    }
                }
            }
            return table;
        }

        public async Task<Table?> GetDataByNameAsync(string TableNumber)
        {
            Table? table = new Table();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetTableByTableNumber", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TableNumber", TableNumber);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        table = (await Reader.ReadAsync()) ? GetDataFromDataBase(Reader) : null;
                    }
                }
            }
            return table;
        }

        public async Task<List<Table>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            List<Table> listTables = new List<Table>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetFilterStatusAndFilterSeatsTables", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", Request.Page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);
                    Command.Parameters.AddWithValue("@StatusID", Request.StatusTableID);
                    Command.Parameters.AddWithValue("@SeatsNumber", Request.Seats);
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            listTables.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return listTables;
        }
        public async Task<List<Table>> GetAllDataAsync(int page)
        {
            List<Table> listTables = new List<Table>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetAllTables", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            listTables.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }
            }
            return listTables;
        }

        public async Task<List<Table>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request)
        {
            List<Table> listTables = new List<Table>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("[Tables].[SP_GetFilterStatusTables]", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", Request.Page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);
                    Command.Parameters.AddWithValue("@StatusID", Request.StatusTableID);
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            listTables.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }

            }
            return listTables;
        }

        public async Task<List<Table>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request)
        {
            List<Table> listTables = new List<Table>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_GetFilterSeatsTables", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", Request.Page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);
                    Command.Parameters.AddWithValue("@Seats", Request.Seats);
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        while (await Reader.ReadAsync())
                        {
                            listTables.Add(GetDataFromDataBase(Reader));
                        }
                    }
                }

            }
            return listTables;
        }
    }

    public class clsTablesRepositoryWriter : clsTablesRepositoryComposition, ITablesRepositoryWriter
    {

        private readonly clsMySettings _Settings;

        public clsTablesRepositoryWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<Table?> CreateDataAsync(DTOTablesCRequest Tables)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_AddTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@StatusTableID", Tables.StatusTableID);
                    Command.Parameters.AddWithValue("@Seats", Tables.Seats);

                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            return (GetDataFromDataBase(Reader));
                        }
                    }
                    return null;
                }
            }
        }
        public async Task<Table?> UpdateDataAsync(DTOTablesURequest Table)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("Tables.SP_UpdateTable", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", Table.ID);
                    Command.Parameters.AddWithValue("@StatusTableID", Table.StatusTableID);
                    Command.Parameters.AddWithValue("@Seats", Table.Seats);
                    
                    await Connection.OpenAsync();
                    using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            return (GetDataFromDataBase(Reader));
                        }
                    }
                    return null;
                }
            }
        }
        public async Task<bool> DeleteDataAsync(int ID)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
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

    public class clsTablesRepository : ITablesRepository
    {
        ITablesRepositoryWriter _IWrite;
        ITablesRepositoryReader _IRead;

        public clsTablesRepository(ITablesRepositoryWriter write, ITablesRepositoryReader read)
        {
            _IWrite = write;
            _IRead = read;
        }


        public async Task<List<Table>> GetAllTablesAvailablesAsync()
        {
            return await _IRead.GetAllDataAvailablesAsync();
        }
        public async Task<List<Table>> GetAlltablesAsync()
        {
            return await _IRead.GetAllDataAsync();
        }
        public async Task<Table?> GetTableAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<Table?> GetTableByNameAsync(string TableNumber)
        {
            return await _IRead.GetDataByNameAsync(TableNumber);
        }

        public async Task<List<Table>> GetFilterStatusAndSeatTablesAsync(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            return await _IRead.GetFilterStatusAndSeatDataAsync(Request);
        }
        public async Task<List<Table>> GetAlltablesAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }

        public async Task<List<Table>> GetFilterStatusTablesAsync(DTOTablesFilterStatusTableRequest Request)
        {
            return await _IRead.GetFilterStatusDataAsync(Request);
        }

        public async Task<List<Table>> GetFilterSeatTablesAsync(DTOTablesFilterSeatTableRequest Request)
        {
            return await _IRead.GetFilterSeatDataAsync(Request);
        }

        public async Task<Table?> AddTableAsync(DTOTablesCRequest Tables)
        {
            return await _IWrite.CreateDataAsync(Tables);
        }
        public async Task<Table?> UpdateTableAsync(DTOTablesURequest Table)
        {
            return await _IWrite.UpdateDataAsync(Table);
        }

        public async Task<bool> DeleteTableAsync(int ID)
        {  
            return await _IWrite.DeleteDataAsync(ID);
        }

    }
}

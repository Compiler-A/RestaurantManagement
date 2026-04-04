using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayerRestaurant
{

    public class DTOTablesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Seats must be a positive integer.")]
        public int Seats { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusTableID must be a positive integer.")]
        public int StatusTableID { get; set; }

        public DTOTablesCRequest( int Seats, int StatusTableID)
        {
            this.Seats = Seats;
            this.StatusTableID = StatusTableID;
        }
    }

    public class DTOTablesURequest : DTOTablesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int ID { get; set; }
        public DTOTablesURequest(int ID,  int Seats, int StatusTableID) 
            : base( Seats, StatusTableID)
        {
            this.ID = ID;
        }
    }

    public class DTOTablesFilterStatusTableRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusTableID must be a positive integer.")]
        public int StatusTableID { get; set; }
    }

    public class DTOTablesFilterSeatTableRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seats must be a positive integer.")]
        public int Seats { get; set; }
    }
    public class DTOTablesFilterStatusAndSeatTableRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "StatusTableID must be a positive integer.")]
        public int StatusTableID { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Seats must be a positive integer.")]
        public int Seats { get; set; }
    }


    public class DTOTables
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
         
        public int ID {  get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seats must be a positive integer.")]
        public int Seats { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusTableID must be a positive integer.")]
        public int StatusTableID { get; set; }

        public DTOStatusTables? StatusTable { get; set; }

        public DTOTables()
        {
            ID = -1;
            Name = string.Empty;
            Seats = -1;
            StatusTable = null;
            StatusTableID = -1;
        }
        public DTOTables(int iD, string table, int seats, int statusTableID)
        {
            ID = iD;
            Name = table;
            Seats = seats;
            StatusTableID = statusTableID;
        }
    }


    public class clsCompositionDTables : ICompositionDataBase<DTOTables>
    {
        public DTOTables GetDataFromDataBase(SqlDataReader Reader)
        {
            return new DTOTables
            {
                ID = Reader.GetInt32(Reader.GetOrdinal("TableID")),
                Name = Reader.GetString(Reader.GetOrdinal("TableNumber")),
                Seats = Reader.GetInt32(Reader.GetOrdinal("Seats")),
                StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID"))
            };
        }
    }

    public class clsTablesReader : clsCompositionDTables ,IReadableDTables
    {

        private readonly clsMySettings _Settings;
        public clsTablesReader(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<List<DTOTables>> GetAllDataAvailablesAsync()
        {
            List<DTOTables> listTables = new List<DTOTables>();
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
        public async Task<List<DTOTables>> GetAllDataAsync()
        {
            List<DTOTables> listTables = new List<DTOTables>();
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
        public async Task<DTOTables?> GetDataAsync(int ID)
        {
            DTOTables? table = new DTOTables();
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

        public async Task<DTOTables?> GetDataByNameAsync(string TableNumber)
        {
            DTOTables? table = new DTOTables();
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

        public async Task<List<DTOTables>> GetFilterStatusAndSeatDataAsync(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            List<DTOTables> listTables = new List<DTOTables>();
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
        public async Task<List<DTOTables>> GetAllDataAsync(int page)
        {
            List<DTOTables> listTables = new List<DTOTables>();
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

        public async Task<List<DTOTables>> GetFilterStatusDataAsync(DTOTablesFilterStatusTableRequest Request)
        {
            List<DTOTables> listTables = new List<DTOTables>();
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

        public async Task<List<DTOTables>> GetFilterSeatDataAsync(DTOTablesFilterSeatTableRequest Request)
        {
            List<DTOTables> listTables = new List<DTOTables>();
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

    public class clsTablesWriter : clsCompositionDTables, IWritableDTables
    {

        private readonly clsMySettings _Settings;

        public clsTablesWriter(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOTables?> CreateDataAsync(DTOTablesCRequest Tables)
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
        public async Task<DTOTables?> UpdateDataAsync(DTOTablesURequest Table)
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

    public class clsDataTables : IDataTables
    {
        IWritableDTables _IWrite;
        IReadableDTables _IRead;

        public clsDataTables(IWritableDTables write, IReadableDTables read)
        {
            _IWrite = write;
            _IRead = read;
        }


        public async Task<List<DTOTables>> GetAllTablesAvailablesAsync()
        {
            return await _IRead.GetAllDataAvailablesAsync();
        }
        public async Task<List<DTOTables>> GetAlltablesAsync()
        {
            return await _IRead.GetAllDataAsync();
        }
        public async Task<DTOTables?> GetTableAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<DTOTables?> GetTableByNameAsync(string TableNumber)
        {
            return await _IRead.GetDataByNameAsync(TableNumber);
        }

        public async Task<List<DTOTables>> GetFilterStatusAndSeatTablesAsync(DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            return await _IRead.GetFilterStatusAndSeatDataAsync(Request);
        }
        public async Task<List<DTOTables>> GetAlltablesAsync(int page)
        {
            return await _IRead.GetAllDataAsync(page);
        }

        public async Task<List<DTOTables>> GetFilterStatusTablesAsync(DTOTablesFilterStatusTableRequest Request)
        {
            return await _IRead.GetFilterStatusDataAsync(Request);
        }

        public async Task<List<DTOTables>> GetFilterSeatTablesAsync(DTOTablesFilterSeatTableRequest Request)
        {
            return await _IRead.GetFilterSeatDataAsync(Request);
        }

        public async Task<DTOTables?> AddTableAsync(DTOTablesCRequest Tables)
        {
            return await _IWrite.CreateDataAsync(Tables);
        }
        public async Task<DTOTables?> UpdateTableAsync(DTOTablesURequest Table)
        {
            return await _IWrite.UpdateDataAsync(Table);
        }

        public async Task<bool> DeleteTableAsync(int ID)
        {  
            return await _IWrite.DeleteDataAsync(ID);
        }

    }
}

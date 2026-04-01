using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System.Collections.Generic;
using System.Runtime;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public class DTOStatusMenusCRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOStatusMenusCRequest(string Name, string? Description) 
        {
            this.Name = Name;
            this.Description = Description;
        }
    }

    public class DTOStatusMenusURequest : DTOStatusMenusCRequest
    {
        public int ID { get; set; }
        public DTOStatusMenusURequest(int ID, string Name,  string? Description) : base(Name, Description)
        {
            this.ID = ID;
        }
    }

    public class DTOStatusMenus
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOStatusMenus()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
        }

        public DTOStatusMenus(int statusMenuID, string statusMenuName, string? statusMenuDescription)
        {
            ID = statusMenuID;
            Name = statusMenuName;
            Description = statusMenuDescription;
        }
    }



    public class clsCompositionDStatusMenus : ICompositionDataBase<DTOStatusMenus>
    {
        public DTOStatusMenus GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOStatusMenus
            {
                ID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }

    public class clsReadableDStatusMenus : clsCompositionDStatusMenus, IReadableDStatusMenus
    {
        private readonly clsMySettings _Settings;

        public clsReadableDStatusMenus(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOStatusMenus?> GetDataAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(_Settings.ConnectionString);
            using SqlCommand command = new SqlCommand("StatusMenus.SP_GetStatusMenusByID", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return GetDataFromDataBase(reader);
            }
            return null;
        }

        public async Task<List<DTOStatusMenus>> GetAllDataAsync(int page)
        {
            var list = new List<DTOStatusMenus>();
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_GetAllStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@PageNumber", page);
            command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(GetDataFromDataBase(reader));
            }
            return list;
        }
    }
    
    public class clsWritableDStatusMenus : clsCompositionDStatusMenus , IWritableDStatusMenus
    {
        private readonly clsMySettings _Settings;

        public clsWritableDStatusMenus(IOptions<clsMySettings> settings)
        {
            _Settings = settings.Value;
        }

        public async Task<DTOStatusMenus?> CreateDataAsync(DTOStatusMenusCRequest statusMenu)
        {
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_AddStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Name", statusMenu.Name);
            command.Parameters.AddWithValue("@Description", (object?)statusMenu.Description ?? DBNull.Value);
            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return GetDataFromDataBase(reader);
                }
            }

            return null;
        }

        public async Task<DTOStatusMenus?> UpdateDataAsync(DTOStatusMenusURequest statusMenu)
        {
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_UpdateStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", statusMenu.ID);
            command.Parameters.AddWithValue("@Name", statusMenu.Name);
            command.Parameters.AddWithValue("@Description", (object?)statusMenu.Description ?? DBNull.Value);

            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return GetDataFromDataBase(reader);
                }
            }

            return null;
        }

        public async Task<bool> DeleteDataAsync(int id)
        {
            using var connection = new SqlConnection(_Settings.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_DeleteStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", id);

            await connection.OpenAsync();
            int rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }

    public class clsDataStatusMenus : IDataStatusMenus
    {
        IReadableDStatusMenus _IRead;
        IWritableDStatusMenus _IWrite;

        public clsDataStatusMenus(IReadableDStatusMenus read, IWritableDStatusMenus write)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<DTOStatusMenus>> GetAllStatusMenusAsync(int Page)
        {
            return await _IRead.GetAllDataAsync(Page);
        }

        public async Task<DTOStatusMenus?> GetStatusMenuAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<DTOStatusMenus?> AddStatusMenuAsync(DTOStatusMenusCRequest Request)
        {
            return await _IWrite.CreateDataAsync(Request);
        }

        public async Task<DTOStatusMenus?> UpdateStatusMenuAsync(DTOStatusMenusURequest Request)
        {
            return await _IWrite.UpdateDataAsync(Request);
        }

        public async Task<bool> DeleteStatusMenuAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }

}




using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public class DTOStatusMenus
    {
        public int StatusMenuID { get; set; }
        public string StatusMenuName { get; set; }
        public string? StatusMenuDescription { get; set; }

        public DTOStatusMenus()
        {
            StatusMenuID = -1;
            StatusMenuName = string.Empty;
            StatusMenuDescription = null;
        }

        public DTOStatusMenus(int statusMenuID, string statusMenuName, string? statusMenuDescription)
        {
            StatusMenuID = statusMenuID;
            StatusMenuName = statusMenuName;
            StatusMenuDescription = statusMenuDescription;
        }
    }

    // Interface
    public interface IDataStatusMenus
    {
        Task<DTOStatusMenus?> GetStatusMenusByID(int statusMenuId);
        Task<List<DTOStatusMenus>> GetAllStatusMenus(int page);
        Task<int> AddStatusMenus(DTOStatusMenus statusMenu);
        Task<bool> UpdateStatusMenu(DTOStatusMenus statusMenu);
        Task<bool> DeleteStatusMenu(int id);
    }

    // Class implementing Interface
    public class clsDataStatusMenus : IDataStatusMenus
    {
        private DTOStatusMenus _MapReader(SqlDataReader reader)
        {
            return new DTOStatusMenus
            {
                StatusMenuID = reader.GetInt32(reader.GetOrdinal("StatusMenuID")),
                StatusMenuName = reader.GetString(reader.GetOrdinal("Name")),
                StatusMenuDescription = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }

        public async Task<DTOStatusMenus?> GetStatusMenusByID(int id)
        {
            using var connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_GetStatusMenusByID", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return _MapReader(reader);
            }
            return null;
        }

        public async Task<List<DTOStatusMenus>> GetAllStatusMenus(int page)
        {
            var list = new List<DTOStatusMenus>();
            using var connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_GetAllStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@PageNumber", page);
            command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(_MapReader(reader));
            }
            return list;
        }

        public async Task<int> AddStatusMenus(DTOStatusMenus statusMenu)
        {
            using var connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_AddStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Name", statusMenu.StatusMenuName);
            command.Parameters.AddWithValue("@Description", (object?)statusMenu.StatusMenuDescription ?? DBNull.Value);

            var outputParam = new SqlParameter("@NewStatusMenuID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(outputParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return outputParam.Value != DBNull.Value ? (int)outputParam.Value : -1;
        }

        public async Task<bool> UpdateStatusMenu(DTOStatusMenus statusMenu)
        {
            using var connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
            using var command = new SqlCommand("StatusMenus.SP_UpdateStatusMenus", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@StatusMenuID", statusMenu.StatusMenuID);
            command.Parameters.AddWithValue("@Name", statusMenu.StatusMenuName);
            command.Parameters.AddWithValue("@Description", (object?)statusMenu.StatusMenuDescription ?? DBNull.Value);

            await connection.OpenAsync();
            int rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteStatusMenu(int id)
        {
            using var connection = new SqlConnection(clsDataAccessLayer.ConnectionString);
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
}

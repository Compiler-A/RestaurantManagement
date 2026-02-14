using Azure;
using Microsoft.Data.SqlClient;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{

    public class DTOJobRoles
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOJobRoles(int ID, string name, string? description)
        {
            this.ID = ID;
            this.Name = name;
            this.Description = description;
        }
        public DTOJobRoles()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
        }
    }

    public interface IReadableJobRolesData
    {
        Task<List<DTOJobRoles>> GetAllJobRoles(int page);
        Task<DTOJobRoles?> GetJobRole(int ID);

    }
    public interface IWritableJobRolesData
    {
        Task<int> Add(DTOJobRoles DTO);
        Task<bool> Update(DTOJobRoles DTO);
        Task<bool> Delete(int ID);
    }

    public interface IDataJobRoles : IReadableJobRolesData, IWritableJobRolesData
    { }

    public class clsDataJobRoles : IDataJobRoles
    {
        private DTOJobRoles _GetSJobRoleFromDataBase(SqlDataReader reader)
        {
            return new DTOJobRoles
            {
                ID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }

        public async Task<List<DTOJobRoles>> GetAllJobRoles(int page)
        {
            List<DTOJobRoles> result = new List<DTOJobRoles>();
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_GetAllJobRoles", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", clsDataAccessLayer.Rows);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader  = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(_GetSJobRoleFromDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<DTOJobRoles?> GetJobRole(int ID)
        {
            DTOJobRoles? result = null;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_GetJobRoleByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = (_GetSJobRoleFromDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Add(DTOJobRoles DTO)
        {
            int result = -1;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_AddJobRole", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", DTO.Name);
                    Command.Parameters.AddWithValue("@Description", (object?)DTO.Description ?? DBNull.Value);
                    SqlParameter output = new SqlParameter("@NewID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    Command.Parameters.Add(output);
                    await Connection.OpenAsync();
                    await Command.ExecuteNonQueryAsync();

                    if (output.Value == null || output.Value == DBNull.Value) 
                        return -1;

                    result = (int)output.Value; 
                }
            }
            return result;
        }

        public async Task<bool> Update(DTOJobRoles DTO)
        {
            bool Updated = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_UpdateJobRole", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", DTO.Name);
                    Command.Parameters.AddWithValue("@Description", (object?)DTO.Description ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@ID", DTO.ID);

                    await Connection.OpenAsync();
                    int Row = await Command.ExecuteNonQueryAsync();

                    Updated = Row > 0;
                }
            }
            return Updated;
        }

        public async Task<bool> Delete(int ID)
        {
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayer.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_DeleteJobRole", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    int Row = await Command.ExecuteNonQueryAsync();

                    Deleted = Row > 0;
                }
            }
            return Deleted;
        }
    }
}
